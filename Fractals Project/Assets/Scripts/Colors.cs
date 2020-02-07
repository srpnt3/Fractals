using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Colors {
    // RGB color (Range: 0 - 1)
    public struct RGBColor {
        public float R, G, B;

        public RGBColor(float R, float G, float B) {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        // RGB to XYZ
        public XYZColor ToXYZ() {
            float r = GammaF(R);
            float g = GammaF(G);
            float b = GammaF(B);
            float X = 0.4124564f * r + 0.3575761f * g + 0.1804375f * b;
            float Y = 0.2126729f * r + 0.7151522f * g + 0.0721750f * b;
            float Z = 0.0193339f * r + 0.1191920f * g + 0.9503041f * b;
            return new XYZColor(X, Y, Z);

            float GammaF(float V) {
                if (V <= 0.04045f)
                    return V / 12.92f;
                else
                    return Mathf.Pow((V + 0.055f) / 1.055f, 2.4f);
            }
        }

        public Vector3 ToVec3() {
            return new Vector3(R, G, B);
        }
    }

    // XYZ Color
    public struct XYZColor {
        public float X, Y, Z;

        public XYZColor(float X, float Y, float Z) {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        // XYZ to RGB
        public RGBColor ToRGB() {
            float R = 3.2404542f * X - 1.5371385f * Y - 0.4985314f * Z;
            float G = -0.969266f * X + 1.8760108f * Y + 0.0415560f * Z;
            float B = 0.0556434f * X - 0.2040259f * Y + 1.0572252f * Z;
            return new RGBColor(GammaF(R), GammaF(G), GammaF(B));

            float GammaF(float V) {
                if (V <= 0.0031308f)
                    return V * 12.92f;
                else
                    return Mathf.Pow(V, 1 / 2.4f) * 1.055f - 0.055f;
            }
        }

        // XYZ to LAB
        public LABColor ToLAB() {
            float x = X / 0.95047f;
            float y = Y / 1f;
            float z = Z / 1.08883f;
            float L = 116 * F(y) - 16;
            float a = 500 * (F(x) - F(y));
            float b = 200 * (F(y) - F(z));
            return new LABColor(L, a, b);

            float F(float V) {
                if (V > 0.008856f)
                    return Mathf.Pow(V, 1 / 3f);
                else
                    return (V * 903.3f + 16) / 116;
            }
        }

        public Vector3 ToVec3() {
            return new Vector3(X, Y, Z);
        }
    }

    // LAB Color
    public struct LABColor {
        public float L, A, B;

        public LABColor(float L, float A, float B) {
            this.L = L;
            this.A = A;
            this.B = B;
        }

        // LAB to XYZ
        public XYZColor ToXYZ() {
            float t = (L + 16) / 116f;
            float x = F(A / 500 + t);
            float z = F(t - B / 200);
            float y;
            if (L > 903.3 * 0.008856)
                y = Mathf.Pow(t, 3);
            else
                y = L / 903.3f;
            return new XYZColor(x * 0.95047f, y * 1f, z * 1.08883f);

            float F(float V) {
                // only works for x and z
                if (Mathf.Pow(V, 3f) > 0.008856f)
                    return Mathf.Pow(V, 3f);
                else
                    return (116 * V - 16) / 903.3f;
            }
        }

        // LAB to LCH
        public LCHColor ToLCH() {
            float C = Mathf.Sqrt(Mathf.Pow(A, 2) + Mathf.Pow(B, 2));
            float t = Mathf.Atan2(B, A) * (180 / Mathf.PI);
            float H;
            if (t >= 0) H = t;
            else H = t + 360;

            return new LCHColor(L, C, H);
        }

        public Vector3 ToVec3() {
            return new Vector3(L, A, B);
        }
    }

    // LCH
    public struct LCHColor {
        public float L, C, H;

        public LCHColor(float L, float C, float H) {
            this.L = L;
            this.C = C;
            this.H = H;
        }

        // LCH to LAB
        public LABColor ToLAB() {
            float A = C * Mathf.Cos(H * (Mathf.PI / 180));
            float B = C * Mathf.Sin(H * (Mathf.PI / 180));
            return new LABColor(L, A, B);
        }

        public Vector3 ToVec3() {
            return new Vector3(L, C, H);
        }
    }
}