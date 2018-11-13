﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using KtaneStuff.Modeling;
using RT.Util;
using RT.Util.Drawing;

namespace KtaneStuff
{
    using static Md;

    static class ColoredSquares
    {
        public static void DoModelsAndTextures()
        {
            File.WriteAllText(@"D:\c\KTANE\ColoredSquares\Assets\Models\Button.obj", GenerateObjFile(Button(), "Button"));
            File.WriteAllText(@"D:\c\KTANE\ColoredSquares\Assets\Models\ButtonHighlight.obj", GenerateObjFile(ButtonHighlight(), "ButtonHighlight"));

            foreach (var (ch, colorCode) in new[] { ('B', "3852E1"), ('G', "38E139"), ('M', "C738E1"), ('R', "E13838"), ('Y', "E1E138") })
            {
                GraphicsUtil.DrawBitmap(256, 256, g =>
                {
                    var color = Color.FromArgb(0xC0, Convert.ToInt32(colorCode.Substring(0, 2), 16), Convert.ToInt32(colorCode.Substring(2, 2), 16), Convert.ToInt32(colorCode.Substring(4, 2), 16));
                    g.Clear(color);
                    g.DrawString(ch.ToString(), new Font("Doris P Bold", 64f, FontStyle.Bold), Brushes.Black, 120, 130, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
                }).Save($@"D:\c\KTANE\ColoredSquares\Assets\Textures\Colorblind-{ch}.png");
            }
        }

        private static IEnumerable<VertexInfo[]> Button()
        {
            var height = .04;
            var fh = height * .4;
            var width = .1;
            var fw = .09;
            var leans = new[] { .001, .001, .001, .001 };
            var bézierSteps = 20;
            var patchPiece = BézierPatch(Ut.NewArray(
                    Ut.NewArray(pt(width, 0, width), pt(width - leans[0], 0, width), pt(fw, 0, width), pt(0, 0, width)),
                    Ut.NewArray(pt(width, 0, width - leans[0]), pt(width - leans[1], fh, width - leans[1]), pt(fw, fh, width - leans[2]), pt(0, fh, width - leans[3])),
                    Ut.NewArray(pt(width, 0, fw), pt(width - leans[2], fh, fw), pt(fw, height, fw), pt(0, height, fw)),
                    Ut.NewArray(pt(width, 0, 0), pt(width - leans[3], fh, 0), pt(fw, height, 0), pt(0, height, 0))
                ),
                bézierSteps);
            //return CreateMesh(false, false, patchPiece);
            return CreateMesh(false, false, Ut.NewArray(2 * bézierSteps - 1, 2 * bézierSteps - 1, (u, v) => new MeshVertexInfo(
                patchPiece[u < bézierSteps ? u : 2 * bézierSteps - 2 - u][v < bézierSteps ? v : 2 * bézierSteps - 2 - v].Apply(p => pt(v < bézierSteps ? p.X : -p.X, p.Y, u < bézierSteps ? p.Z : -p.Z)),
                u == 2 * bézierSteps - 2 ? Normal.Mine : Normal.Average,
                u == 0 ? Normal.Mine : Normal.Average,
                v == 2 * bézierSteps - 2 ? Normal.Mine : Normal.Average,
                v == 0 ? Normal.Mine : Normal.Average,
                p(v / (double) (2 * bézierSteps), 1 - u / (double) (2 * bézierSteps)))));
        }

        private static IEnumerable<VertexInfo[]> ButtonHighlight()
        {
            var width = .11;
            var innerWidth = .09;
            return CreateMesh(false, true, Ut.NewArray(2, 4, (i, j) => (j * 90 + 45).Apply(angle => (i == 0 ? innerWidth : width).Apply(radius => pt(radius * cos(angle), 0, radius * sin(angle))))));
        }
    }
}
