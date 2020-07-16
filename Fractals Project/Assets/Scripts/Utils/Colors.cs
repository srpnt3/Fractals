using UnityEngine;

public static class Colors {
    
    // conversions from http://www.brucelindbloom.com/index.html?Math.html
    
    // RGB color (Range: 0 - 1)
    public struct RGBColor {
        public float R, G, B;

        public RGBColor(float r, float g, float b) {
            R = r;
            G = g;
            B = b;
        }

        // RGB to XYZ
        public XYZColor ToXYZ() {
            float r = GammaF(R);
            float g = GammaF(G);
            float b = GammaF(B);
            // transformation matrix
            float x = 0.4124564f * r + 0.3575761f * g + 0.1804375f * b;
            float y = 0.2126729f * r + 0.7151522f * g + 0.0721750f * b;
            float z = 0.0193339f * r + 0.1191920f * g + 0.9503041f * b;
            return new XYZColor(x, y, z);

            // inverse companding
            float GammaF(float v) {
                if (v <= 0.04045f)
                    return v / 12.92f;
                return Mathf.Pow((v + 0.055f) / 1.055f, 2.4f);
            }
        }

        public Vector3 ToVec3() {
            return new Vector3(R, G, B);
        }

        public static RGBColor Random() {
            return new RGBColor(UnityEngine.Random.Range(0f, 1), UnityEngine.Random.Range(0f, 1), UnityEngine.Random.Range(0f, 1));
        }
    }

    // XYZ Color
    public struct XYZColor {
        public float X, Y, Z;

        public XYZColor(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }

        // XYZ to RGB
        public RGBColor ToRGB() {
            // transformation matrix
            float r = 3.2404542f * X - 1.5371385f * Y - 0.4985314f * Z;
            float g = -0.969266f * X + 1.8760108f * Y + 0.0415560f * Z;
            float b = 0.0556434f * X - 0.2040259f * Y + 1.0572252f * Z;
            return new RGBColor(GammaF(r), GammaF(g), GammaF(b));

            // companding
            float GammaF(float v) {
                if (v <= 0.0031308f)
                    return v * 12.92f;
                return Mathf.Pow(v, 1 / 2.4f) * 1.055f - 0.055f;
            }
        }

        // XYZ to LAB
        public LABColor ToLAB() {
            // reference white
            float x = X / 0.95047f;
            float y = Y / 1f;
            float z = Z / 1.08883f;
            
            float l = 116 * F(y) - 16;
            float a = 500 * (F(x) - F(y));
            float b = 200 * (F(y) - F(z));
            return new LABColor(l, a, b);

            float F(float v) {
                if (v > 0.008856f)
                    return Mathf.Pow(v, 1 / 3f);
                return (v * 903.3f + 16) / 116;
            }
        }

        public Vector3 ToVec3() {
            return new Vector3(X, Y, Z);
        }
    }

    // LAB Color
    public struct LABColor {
        public float L, A, B;

        public LABColor(float l, float a, float b) {
            L = l;
            A = a;
            B = b;
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
            
            // return and reference white
            return new XYZColor(x * 0.95047f, y * 1f, z * 1.08883f);

            float F(float v) {
                // only works for x and z
                if (Mathf.Pow(v, 3f) > 0.008856f)
                    return Mathf.Pow(v, 3f);
                return (116 * v - 16) / 903.3f;
            }
        }

        // LAB to LCH
        public LCHColor ToLCH() {
            float c = Mathf.Sqrt(Mathf.Pow(A, 2) + Mathf.Pow(B, 2));
            float t = Mathf.Atan2(B, A) * (180 / Mathf.PI);
            float h;
            if (t >= 0) h = t;
            else h = t + 360;

            return new LCHColor(L, c, h);
        }

        public Vector3 ToVec3() {
            return new Vector3(L, A, B);
        }
    }

    // LCH
    public struct LCHColor {
        public float L, C, H;

        public LCHColor(float l, float c, float h) {
            L = l;
            C = c;
            H = h;
        }

        // LCH to LAB
        public LABColor ToLAB() {
            float a = C * Mathf.Cos(H * (Mathf.PI / 180));
            float b = C * Mathf.Sin(H * (Mathf.PI / 180));
            return new LABColor(L, a, b);
        }

        public Vector3 ToVec3() {
            return new Vector3(L, C, H);
        }
    }
}