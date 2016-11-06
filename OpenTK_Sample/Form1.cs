using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_Sample
{
    /// <summary>
    /// refer  : http://ameblo.jp/nishi-u6fa4/entry-10659712465.html
    /// openTK : (nuget)install-package opentk
    /// glControl : create binary(glControl.dll) from https://github.com/opentk/opentk source. 
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // 領域のサイズが(300, 300) のとき，現在の正方形の1辺は - 0.5～0.5 の 長さ 1 なので，150 pixel = formのsizeからの相対値
            // 領域のサイズが(300, 150) のとき，正方形の縦の長さは 75 pixel = formのsizeの単位はpixel
            // (コントロールの幅) / (コントロールの高さ) はアスペクト比と呼ばれるもので，これを使えば x = y * (アスペクト比) となります。
            this.Width = 300;
            this.Height = 150;
        }

        protected override void OnLoad(EventArgs e)
        {
            // base.OnLoad(e);
            // カラーバッファを消去する際の色を指定します。
            GL.ClearColor(glControl.BackColor);

            // ビューポート設定
            // 画像が実際に描画される領域（ビューポート）を指定します。
            // (x, y) は左下隅を表し，(width, height) がサイズを表します。
            // Form コントロールでは左上隅が (0, 0) で下向きが正になりますが，
            // OpenGL 領域では左下隅が (0, 0) で上向きが正になることに注意してください。
            GL.Viewport(0, 0, glControl.Width, glControl.Height);

            // 視体積（見え方：カメラのレンズを広角レンズとか魚眼レンズとかに取り替える操作）の設定
            // 現在の行列を mode で指定する行列に切り替えます。
            GL.MatrixMode(MatrixMode.Projection);
            float h = 4.0f;
            float w = h * glControl.AspectRatio;
            // Matrix4.CreateOrthographic(float width, float height, float zNear, float zFar)
            // Projection を指定する行列を作成します。
            // (width, height) がサイズを表し，視点に対して zNear～zFar の領域だけ描画します。
            // zNear に 0.0f を指定できないので 0.01f という小さい値を指定しています。
            Matrix4 proj = Matrix4.CreateOrthographic(w, h, 0.01f, 2.0f);
            // 現在の行列を mat に置き換えます。
            GL.LoadMatrix(ref proj);

            // 視界の設定
            GL.MatrixMode(MatrixMode.Modelview);
            // Matrix4.LookAt(Vector3 eye, Vector3 target, Vector3 up)
            // 視点を表す行列を作成します
            // Vector3 は (x, y, z) の座標3つを保存できる変数です。
            // Vector3.Zero は (0.0f, 0.0f, 0.0f)
            // Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ はそれぞれ (1.0f, 0.0f, 0.0f), (0.0f, 1.0f, 0.0f), (0.0f, 0.0f, 1.0f) の座標を表します。
            // eye には視点の場所，target には視点が見る点，up には上向きの方向を指定します。
            Matrix4 look = Matrix4.LookAt(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY);
            // Matrix4 look = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 0.1f), new Vector3(0.1f, 0.1f, 0.0f), Vector3.UnitY);
            GL.LoadMatrix(ref look);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            // mask で指定したバッファを消去します。
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // アスペクト比算出
            float r0 = glControl.Width / glControl.Height;
            float r1 = glControl.AspectRatio;
            Console.WriteLine("calc = {0}, method = {1}", r0, r1);

            // 図形の描画を開始します。
            // ここから End までの間で指定された点を用いて，
            // mode で指定された方法で図を描画します。
            GL.Begin(BeginMode.TriangleStrip);
            {
                // 頂点の座標を x, y で指定します

                // 座標を変換するversion
                //GL.Vertex2(0.5 / r1, 0.5);
                //GL.Vertex2(-0.5 / r1, 0.5);
                //GL.Vertex2(0.5 / r1, -0.5);
                //GL.Vertex2(-0.5 / r1, -0.5);

                // 描画領域を変換するversion
                GL.Vertex2(0.5, 0.5);
                GL.Vertex2(-0.5, 0.5);
                GL.Vertex2(0.5, -0.5);
                GL.Vertex2(-0.5, -0.5);

            }
            // 図形描画の終了点です。
            GL.End();

            // バックグラウンドで描画した画面を現在の画面と入れ替えます。（コントロールに対して行うことに注意）
            glControl.SwapBuffers();

        }
    }
}
