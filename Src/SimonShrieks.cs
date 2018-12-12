﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KtaneStuff.Modeling;
using RT.Util.ExtensionMethods;
using RT.Util.Geometry;

namespace KtaneStuff
{
    using static Md;

    static class SimonShrieks
    {
        public static void DoModels()
        {
            File.WriteAllText($@"D:\c\KTANE\SimonShrieks\Assets\Models\Button.obj", GenerateObjFile(Button(), "Button"));
            File.WriteAllText($@"D:\c\KTANE\SimonShrieks\Assets\Models\ButtonHighlight.obj", GenerateObjFile(ButtonHighlight(), "ButtonHighlight"));
            File.WriteAllText($@"D:\c\KTANE\SimonShrieks\Assets\Models\ButtonCollider.obj", GenerateObjFile(ButtonCollider(), "ButtonCollider"));
            File.WriteAllText($@"D:\c\KTANE\SimonShrieks\Assets\Models\Arrow1.obj", GenerateObjFile(Arrow1(), "Arrow1"));
            File.WriteAllText($@"D:\c\KTANE\SimonShrieks\Assets\Models\Arrow2.obj", GenerateObjFile(Arrow2(), "Arrow2"));
            File.WriteAllText($@"D:\c\KTANE\SimonShrieks\Assets\Models\Arrow3.obj", GenerateObjFile(Arrow3(), "Arrow3"));
        }

        private static IEnumerable<VertexInfo[]> Arrow1()
        {
            yield return new[] { pt(-.5, 0, .75), pt(0, 0, 1.75), pt(.5, 0, .75) }.Select(p => p.WithNormal(0, 1, 0)).ToArray();
        }

        private static IEnumerable<VertexInfo[]> Arrow2()
        {
            yield return new[] { pt(-.5, 0, .65), pt(0, 0, 1.25), pt(.5, 0, .65) }.Select(p => p.WithNormal(0, 1, 0)).ToArray();
            yield return new[] { pt(-.5, 0, 1.25), pt(0, 0, 1.85), pt(.5, 0, 1.25) }.Select(p => p.WithNormal(0, 1, 0)).ToArray();
        }

        private static IEnumerable<VertexInfo[]> Arrow3()
        {
            yield return new[] { pt(-.5, 0, .45), pt(0, 0, .95), pt(.5, 0, .45) }.Select(p => p.WithNormal(0, 1, 0)).ToArray();
            yield return new[] { pt(-.5, 0, .95), pt(0, 0, 1.45), pt(.5, 0, .95) }.Select(p => p.WithNormal(0, 1, 0)).ToArray();
            yield return new[] { pt(-.5, 0, 1.45), pt(0, 0, 1.95), pt(.5, 0, 1.45) }.Select(p => p.WithNormal(0, 1, 0)).ToArray();
        }

        private static Pt offset(Pt p) => p + .4 * pt(-cos(360.0 / 7 / 2), 0, sin(360.0 / 7 / 2));

        private static IEnumerable<VertexInfo[]> Button()
        {
            const int steps = 8;

            var v1 = pt(0, 0, 0);
            var v2 = pt(0, 0, .5);
            var v3 = pt(0, 0, 1);
            var angle = 360.0 / 7.0;
            var h = 1.15;
            var v4 = pt(h * cos(90 - angle), 0, h * sin(90 - angle));
            var c = ((v1 + v3 + v4) / 3).Add(y: .5);

            var patchRaw = BézierPatch(
                v1, (2 * v1 + v2) / 3, (v1 + 2 * v2) / 3, v2,
                (2 * v1 + v4) / 3, (c + v1) / 2, (c + v2) / 2, (2 * v2 + v3) / 3,
                (v1 + 2 * v4) / 3, (c + v4) / 2, (c + v3) / 2, (v2 + 2 * v3) / 3,
                v4, (2 * v4 + v3) / 3, (v4 + 2 * v3) / 3, v3,
                steps
            );
            var patch = patchRaw
                .Select((arr, x) => arr.Select((p, y) => offset(p).WithMeshInfo(
                    x == 0 || x == patchRaw.Length - 1 ? Normal.Mine : Normal.Average,
                    x == 0 || x == patchRaw.Length - 1 ? Normal.Mine : Normal.Average,
                    y == 0 || y == patchRaw[0].Length - 1 ? Normal.Mine : Normal.Average,
                    y == 0 || y == patchRaw[0].Length - 1 ? Normal.Mine : Normal.Average)).ToArray()).Reverse().ToArray();

            return CreateMesh(false, false, patch);
        }

        private static IEnumerable<Pt[]> ButtonHighlight()
        {
            var v1 = pt(0, 0, 0);
            var v2 = pt(0, 0, .5);
            var v3 = pt(0, 0, 1);
            var angle = 360.0 / 7.0;
            var h = 1.15;
            var v4 = pt(h * cos(90 - angle), 0, h * sin(90 - angle));
            var c = ((v1 + v3 + v4) / 3);

            yield return new[] { v1, v4, v3 }.Select(p => offset((p - c) * 1.25 + c)).ToArray();
        }

        private static IEnumerable<VertexInfo[]> ButtonCollider()
        {
            PointD offset(PointD pt) => pt + .4 * p(-cos(360.0 / 7 / 2), sin(360.0 / 7 / 2));

            var v1 = p(0, 0);
            var v3 = p(0, 1);
            var angle = 360.0 / 7.0;
            var h = 1.15;
            var v4 = p(h * cos(90 - angle), h * sin(90 - angle));
            var c = ((v1 + v3 + v4) / 3);

            return new[] { v1, v4, v3 }.Select(p => offset(p)).Extrude(.25, true, true);
        }

        public static void CreateCheatSheet()
        {
            var colorNames = "RYGCBWM";
            int[][] _grid = new[]
            {
                "GMCBYRCYBWR",
                "GWCWMYRWWRC",
                "YBWGGCWBRWM",
                "BRMYCRYGMBR",
                "WCYBGBRWGYC",
                "GYRCMRMGWRB",
                "YMGCMMGBCMW",
                "MBRYGYBWYRW",
                "RCYBCBGRCBM",
                "YYMGBMCYWCW",
                "WCGMRGCMGBB"
            }
                .Select(arr => arr.Select(ch => colorNames.IndexOf(ch)).ToArray()).ToArray();

            var xs = new[] { 2, 8, 2, 8, 2, 8, 5 };
            var ys = new[] { 2, 2, 8, 8, 5, 5, 5 };

            var svg = $@"
                <svg viewBox='-.025 -.025 7.05 7.05' text-anchor='middle'>

                    <!-- Frame -->
                    <rect x='0' y='0' width='7' height='7' stroke-width='.05' fill='none' stroke='#000' />

                    <!-- Starting locations -->
                    <g stroke='none' fill='#ccc' font-size='1.2'>
                        {Enumerable.Range(0, 7).Select(col => $"<text x='{xs[col] - 2 + .5}' y='{ys[col] - 2 + .925}'>{col}</text>").JoinString()}
                    </g>

                    <!-- Grid lines -->
                    <g stroke-width='.01' stroke-dasharray='.01, .02' fill='none' stroke='#000'>
                        {Enumerable.Range(1, 6).Select(i => $"<line x1='0' y1='{i}' x2='7' y2='{i}' /><line x1='{i}' y1='0' x2='{i}' y2='7' />").JoinString()}
                    </g>

                    <!-- Answers (text objects) -->
                    <g font-size='.2'>
                        {Enumerable.Range(2, 7).SelectMany(cy =>
                            Enumerable.Range(2, 7).Select(cx =>
                            {
                                var countColors = new int[7];
                                var firstOccurrence = new int[7];
                                var cells = 25;
                                for (int y = 2; y >= -2; y--)
                                    for (int x = 2; x >= -2; x--)
                                    {
                                        countColors[_grid[cy + y][cx + x]]++;
                                        firstOccurrence[_grid[cy + y][cx + x]] = cells;
                                        cells--;
                                    }

                                string answer(bool hasVowel)
                                {
                                    var colorsToPress = Enumerable.Range(0, 7).Where(ix => countColors[ix] % 2 == (hasVowel ? 0 : 1)).ToArray();
                                    Array.Sort(colorsToPress, (v1, v2) =>
                                        countColors[v1] > countColors[v2] ? 1 :
                                        countColors[v1] < countColors[v2] ? -1 :
                                        firstOccurrence[v1] > firstOccurrence[v2] ? 1 :
                                        firstOccurrence[v1] < firstOccurrence[v2] ? -1 : 0);
                                    return colorsToPress.Select(c => colorNames[c]).JoinString("");
                                }

                                return $"<text x='{cx - 2 + .5}' y='{cy - 2 + .425}'>{answer(true)}</text><text x='{cx - 2 + .5}' y='{cy - 2 + .675}'>{answer(false)}</text>";
                            })
                        ).JoinString()}
                    </g>

                    <!-- Directions -->
                    <g fill='#000' stroke='none' font-size='.1'>
                        {new Func<string>(() =>
                        {
                            var results = new List<string>();
                            for (int cx = 2; cx <= 8; cx++)
                                for (int cy = 2; cy <= 8; cy++)
                                {
                                    var d = new Dictionary<(int mx, int my), List<int>>();
                                    foreach (var col in Enumerable.Range(0, 7))
                                    {
                                        int nx = xs[col], ny = ys[col];
                                        int dx = Math.Abs(cx - nx), dy = Math.Abs(cy - ny), sx = Math.Sign(nx - cx), sy = Math.Sign(ny - cy);
                                        int mx = 0, my = 0;

                                        if (dx >= dy)
                                            mx += sx;
                                        if (dx <= dy)
                                            my += sy;

                                        if (mx != 0 || my != 0)
                                            d.AddSafe((mx, my), col);
                                    }

                                    var xOffsets = new[] { .05, .5, .95 };
                                    var yOffsets = new[] { .15, .55, .95 };
                                    var anchors = new[] { "start", "middle", "end" };
                                    const double yo = .1;
                                    for (int mx = -1; mx <= 1; mx++)
                                        for (int my = -1; my <= 1; my++)
                                            if (d.ContainsKey((mx, my)))
                                                results.Add(my == 0
                                                    ? d[(mx, my)].Select((str, ix) => $"<text x='{cx - 2 + xOffsets[mx + 1]}' y='{cy - 2 + yOffsets[my + 1] - yo * d[(mx, my)].Count / 2 + yo * ix}' text-anchor='{anchors[mx + 1]}'>{str}</text>").JoinString()
                                                    : $@"<text x='{cx - 2 + xOffsets[mx + 1]}' y='{cy - 2 + yOffsets[my + 1]}' text-anchor='{anchors[mx + 1]}'>{d[(mx, my)].JoinString(" ")}</text>");
                                }

                            return results.JoinString();
                        })()}
                    </g>
                </svg>
            ";

            Utils.ReplaceInFile(@"D:\c\KTANE\Public\HTML\Simon Shrieks optimized (Timwi).html", "<!-- #start -->", "<!-- #end -->", svg);
        }
    }
}