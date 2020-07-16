using System;
using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using UnityEngine;

public class AudioProvider {

	private WasapiLoopbackCapture capture;
	private SoundInSource source;
	private IWaveSource sound;
	private SingleBlockNotificationStream stream;
	private FftProvider fftProvider;
	private readonly Action<float[]> callback;
	private readonly int size;
	private readonly int minF;
	private readonly int maxF;
	
	public AudioProvider(int size, int minFrequency, int maxFrequency, Action<float[]> callback) {
		this.size = size;
		minF = minFrequency;
		maxF = maxFrequency;
		this.callback = callback;
	}

	// start capturing
	public void Start() {
		capture = new WasapiLoopbackCapture();
		capture.Initialize();
		
		source = new SoundInSource(capture);

		fftProvider = new FftProvider(source.WaveFormat.Channels, FftSize.Fft4096);
		
		capture.Start();
		
		stream = new SingleBlockNotificationStream(source.ToSampleSource());
		sound = stream.ToWaveSource().ToMono();
		
		var buffer = new byte[source.WaveFormat.BytesPerSecond / 2];

		source.DataAvailable += (s, ea) => {
			while (sound.Read(buffer, 0, buffer.Length) > 0) {
				var fftBuffer = new float[(int) FftSize.Fft4096];
				if (fftProvider.GetFftData(fftBuffer)) {
					ConvertData(fftBuffer);
				}
			}
		};

		stream.SingleBlockRead += SingleBlockReadEvent;
	}

	// stop capturing
	public void Stop() {
		stream.SingleBlockRead -= SingleBlockReadEvent;
		source.Dispose();
		sound.Dispose();
		capture.Stop();
		capture.Dispose();
	}

	// map the results
	private void ConvertData(float[] audio) {
		int f = source.WaveFormat.SampleRate / 2;
		float[] data = new float[size];
		for (int i = minF; i <= maxF; i++) { // loop through all frequencies
			int i1 = Mathf.RoundToInt((float) (i - minF) / (maxF - minF) * (size - 1)); // index in "data" array
			int i2 = Mathf.RoundToInt((float) i / f * ((float) FftSize.Fft4096 / 2 - 1)); // index in "audio" array
			data[i1] = Mathf.Max(data[i1], Mathf.Sqrt(audio[i2]) * 2);
		}
		callback(data);
	}

	private void SingleBlockReadEvent(object s, SingleBlockReadEventArgs e) => fftProvider.Add(e.Left, e.Right);
}