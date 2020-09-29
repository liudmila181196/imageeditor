using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Numerics;

using ZedGraph;
using System.Linq;
using System.IO;
using System.Media;
using System.Threading;

namespace Nazarova
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            /*
                        Model model = new Model();
                        RdWr rdwr = new RdWr();
                        Analysis analysis = new Analysis();
                        Processing proc = new Processing();
                        Dictionary<PointPairList, String> list = new Dictionary<PointPairList, string>();

                        list = Program.trend(model);
                        list = Program.trendNorm(model, rdwr);
                        list = Program.random(model);
                        list = Program.stat(model);
                        Application.Run(new RdWr(list, 0.005));
                        list = Program.addMulti(model);
                        list = Program.multiChannel(model);*//*
                        list = Program.histogram(model);
                        list = Program.correlation(model);
                        list = Program.sinDiffFreq(model);
                        list = Program.specter(model);
                        list = Program.sum3sin(model);
                        list = Program.delAndAnti(model);
                        list = Program.shiftSin(model);
                        list = Program.trendSin(model);
                        list = Program.randAndSpikeSpecter(model);
                        String name = "E:\\data.dat";
                        PointPairList listFile = rdwr.readFile(name);
                        list = Program.procFile(listFile);
                        Dictionary<PointPairList, String> list1 = Program.getComplex(model);
                        Dictionary<PointPairList, String> list2 = Program.specZero(model);
                        Application.Run(new RdWr(list1));
                        Application.Run(new RdWr(list2));

                        Dictionary<PointPairList, String> list1 = Program.cardio(model);
                        Dictionary<PointPairList, String> list1 = Program.cardio2(model);
                        String name = "E:\\data.dat";
                        PointPairList listFile = rdwr.readFile(name);
                        //Dictionary<PointPairList, String> list1 = Program.filter(model, analysis, proc, listFile);
                        Dictionary<PointPairList, String> list1 = Program.filterS(model, analysis, proc, listFile);
                        Dictionary<PointPairList, String> list1 = Program.wav(model, analysis, proc);
                        Dictionary<PointPairList, String> list1 = Program.wavMy(model, analysis, proc);
                        String name = "E:\\v1z1.dat";
                        PointPairList listFile = rdwr.readFile(name);
                        Dictionary<PointPairList, String> list1 = Program.zachet(model, analysis, proc, listFile);
                        Application.Run(new RdWr(list1));*/

            //Dictionary<PointPairList, String> list1 = Program.furieObr(model, analysis, proc);
            //Application.Run(new RdWr(list1));

            //Bitmap bitmap = resizeIm(proc);
            //Bitmap bitmap = contrast(proc);
            //Application.Run(new RdWrImage(bitmap));

            //Dictionary<PointPairList, String> list1 = Program.imageHistogram(model, analysis, proc);
            //Bitmap bitmap = Program.correctionCDF(proc, analysis);
            //Application.Run(new RdWrImage(bitmap));

            //Bitmap bitmap = Program.xrayCDF(proc, analysis);
            //Application.Run(new RdWrImage(bitmap));
            //Dictionary<PointPairList, String> list1 = Program.xray(model, analysis, proc);
            //Dictionary<PointPairList, String> list1 = Program.noiseImg(model, analysis, proc);
            //Bitmap bitmap = Program.normNoise(proc, analysis);
            //Bitmap bitmap = Program.SPNoise(proc, analysis);
            //Bitmap bitmap = Program.SUMNoise(proc, analysis);
            //Bitmap bitmap = Program.MMFilter(proc);
            //Bitmap bitmap = Program.blur(proc,analysis,rdwr);
            //Dictionary<PointPairList, String> list1 = Program.histoModel(analysis, proc);
            //Bitmap bitmap = Program.contourLPF(proc, analysis, rdwr, model);
            //Bitmap bitmap = Program.contourLPF(proc, analysis, rdwr, model);
            //Bitmap bitmap = Program.laplasian(proc, analysis, rdwr);
            //Bitmap bitmap = Program.dilatation(proc, analysis, rdwr);
            //Bitmap bitmap = Program.erosion(proc, analysis, rdwr);
            //Bitmap bitmap = Program.MRT(proc, analysis, rdwr);
            //Bitmap bitmap = Program.stones(proc, analysis, rdwr, model);
            //Application.Run(new RdWrImage(bitmap));
            //Dictionary<PointPairList, String> list1 = Program.histoModel3(analysis, proc);
            //Application.Run(new RdWr(list1));
            Application.Run(new ImageEdior());
        }

        public static Bitmap stones(Processing proc, Analysis analysis, RdWr rdwr, Model model)
        {
            
            Image im = Image.FromFile("D:\\stones.jpg");
            /*
            Image imT = proc.globalThresholdConversion(im, 110, 0, 255);
            imT.Save("D:\\stones_T.jpg");
            int m = 5;
            int[] mask1 = new int[m*m];
            for (int i = 0; i < m*m; i++)
            {
                mask1[i] = 255;
            }
            imT = proc.erosion(imT, mask1, m);
            imT.Save("D:\\stones_E.jpg");
            imT = proc.dilatation(imT, mask1, m);
            imT.Save("D:\\stones_TR.jpg");*/
            Image imT = Image.FromFile("D:\\stones_TR.jpg");
            int n = 12, frame =2;
            /*PointPairList[] mask = model.roundMask(n, frame, 100, 250);
            Image mask_im = proc.listToImage(mask, n + frame*2, n + frame*2);
            mask_im.Save("D:\\mask.jpg");*/
            PointPairList[] mask = proc.imageToList(Image.FromFile("D:\\mask.jpg"));
            /*Image im_er = proc.erosion2(imT, mask, n + frame * 2, 100, 0.85, 0.05);
            im_er.Save("D:\\stones_Er99.jpg");
            
            
            mask = model.roundMask(n, frame, 100, 250);
            Image mask_im = proc.listToImage(mask, n + frame * 2, n + frame * 2);
            mask_im.Save("D:\\mask2.jpg");*/
            n = 2; frame =5;
            mask = proc.imageToList(Image.FromFile("D:\\mask2.jpg"));
            Image im_er = Image.FromFile("D:\\stones_Er99.jpg");
            im_er = proc.numberOfStones2(im_er, mask, n + frame * 2, 100);
            im_er.Save("D:\\stones_1.jpg");

            Bitmap bitmap = CombineBitmap(new[] { "D:\\stones_T.jpg", "D:\\stones_TR.jpg"});

            return bitmap;
        }
        public static Bitmap optimalImage(Image image, Processing proc, Analysis analysis, String name, int T, int maskSize)
        {
            PointPairList list = analysis.imgCDF(image);
            Image im_CDF = proc.corrCDF(image, list);
            /*PointPairList[] imageList = proc.imageToList(im_CDF);
            imageList = proc.grayScale(imageList, image.Width, image.Height);
            im_CDF = proc.listToImage(imageList, image.Width, image.Height);
            

            Image imT = proc.globalThresholdConversion(image, T);
            Image imDil = proc.dilatation2(imT, maskSize);
            

            im_CDF = proc.substractImAndMask(im_CDF, imDil);*/
            im_CDF.Save("D:\\" + name + "_Result.jpg");

            Bitmap bitmap = CombineBitmap(new[] {  "D:\\" + name + "_Result.jpg" });
            return bitmap;
        }
        public static Dictionary<PointPairList, String> histoModel3(Analysis analysis, Processing proc)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Image im = Image.FromFile("D:\\stones.jpg");
            PointPairList histo = analysis.imgHistogram(im);
            list.Add(histo, "");

            return list;
        }
        
        public static Bitmap MRT(Processing proc, Analysis analysis, RdWr rdwr)
        {
            /*
            PointPairList[] spine_H = rdwr.readFileArray2("D:\\spine-H_x256.bin", 256, 256);
            spine_H = proc.grayScale(spine_H, 256, 256);
            Image spine_H_Im = proc.listToImage(spine_H, 256, 256);
            spine_H_Im.Save("D:\\images\\spine_H.jpg");
            Bitmap bitmap = optimalImage(spine_H_Im, 400, 400, proc, analysis, "spine_H", 5, 5);
            
            
            PointPairList[] spine_V = rdwr.readFileArray2("D:\\spine-V_x512.bin", 512, 512);
            spine_V = proc.grayScale(spine_V, 512, 512);
            Image spine_V_Im = proc.listToImage(spine_V, 512, 512);
            spine_V_Im.Save("D:\\images\\spine_V.jpg");
            Bitmap bitmap = optimalImage(spine_V_Im, 400, 400, proc, analysis, "spine_V", 7, 5);
            bitmap = CombineBitmap(new[] { "D:\\images\\spine_V.jpg", "D:\\images\\spine_V_Mask.jpg" });
            //bitmap = CombineBitmap(new[] { "D:\\images\\spine_V_Result.jpg", "D:\\images\\spine_V_resize.jpg" });
            */
            
            PointPairList[] brain_V = rdwr.readFileArray2("D:\\brain-V_x256.bin", 256, 256);
            brain_V = proc.grayScale(brain_V, 256, 256);
            Image brain_V_Im = proc.listToImage(brain_V, 256, 256);
            brain_V_Im.Save("D:\\images\\brain_V.jpg");
            Bitmap bitmap = optimalImage(brain_V_Im, 400, 400, proc, analysis, "brain_V", 14, 3);
            /*
            PointPairList[] brain_H = rdwr.readFileArray2("D:\\brain-H_x512.bin", 512, 512);
            brain_H = proc.grayScale(brain_H, 512, 512);
            Image brain_H_Im = proc.listToImage(brain_H, 512, 512);
            brain_H_Im.Save("D:\\images\\brain_H.jpg");
            Bitmap bitmap = optimalImage(brain_H_Im, 400, 400, proc, analysis, "brain_H", 11, 5);
            //bitmap = CombineBitmap(new[] { "D:\\images\\brain_H.jpg", "D:\\images\\brain_H_Mask.jpg" });
            bitmap = CombineBitmap(new[] { "D:\\images\\brain_H_Result.jpg", "D:\\images\\brain_H_resize.jpg" });*/

            return bitmap;
        }
        public static Bitmap optimalImage (Image image, int neww, int newh, Processing proc, Analysis analysis, String name, int T, int maskSize)
        {
            PointPairList list = analysis.imgCDF(image);
            Image im_CDF = proc.corrCDF(image, list);
            PointPairList[] imageList = proc.imageToList(im_CDF);
            imageList = proc.grayScale(imageList, image.Width, image.Height);
            im_CDF = proc.listToImage(imageList, image.Width, image.Height);
            im_CDF.Save("D:\\images\\" + name + "_CDF.jpg");

            Image imT = proc.globalThresholdConversion(image, T);
            Image imDil = proc.dilatation2(imT, maskSize);
            imDil.Save("D:\\images\\" + name + "_Mask.jpg");

            im_CDF = proc.substractImAndMask(im_CDF, imDil);
            im_CDF.Save("D:\\images\\" + name + "_Result.jpg");

            im_CDF = proc.nearestNeighbors(im_CDF, neww, newh);
            im_CDF.Save("D:\\images\\" + name + "_resize.jpg");

            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\" + name + ".jpg", "D:\\images\\" + name + "_Mask.jpg", "D:\\images\\" + name + "_Result.jpg", "D:\\images\\" + name + "_resize.jpg" });
            return bitmap;
        }

        public static Bitmap erosion(Processing proc, Analysis analysis, RdWr rdwr)
        {
            /*
            Image im = Image.FromFile("D:\\images\\gThreshold_1.jpg");
            double[] mask = { 255, 255, 255, 255, 255, 255, 255, 255, 255 };
            Image imEro = proc.erosion(im, mask, 3);
            imEro.Save("D:\\images\\MODEL_Erosion.jpg");
            PointPairList[] imList = proc.imageToList(im);
            PointPairList[] imEroList = proc.imageToList(imEro);
            imEroList = proc.subtractionList(imList, imEroList, im.Height, im.Width);
            imEro = proc.listToImage(imEroList, im.Height, im.Width);
            imEro.Save("D:\\images\\MODEL_EroSub.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_1.jpg", "D:\\images\\MODEL_Erosion.jpg", "D:\\images\\MODEL_EroSub.jpg" });
          
            Image im = Image.FromFile("D:\\images\\gThreshold_gauss15.jpg");
            double[] mask = new double [25];
            for (int i=0; i<25; i++)
            {
                mask[i] = 255;
            }
            Image imEro = proc.erosion(im, mask, 5);
            imEro.Save("D:\\images\\gThreshold_gauss15_Ero.jpg");
            PointPairList[] imList = proc.imageToList(im);
            PointPairList[] imEroList = proc.imageToList(imEro);
            imEroList = proc.subtractionList(imList, imEroList, im.Height, im.Width);
            imEro = proc.listToImage(imEroList, im.Height, im.Width);
            imEro.Save("D:\\images\\gThreshold_gauss15_EroSub.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_gauss15.jpg", "D:\\images\\gThreshold_gauss15_Ero.jpg", "D:\\images\\gThreshold_gauss15_EroSub.jpg" });
 
            Image im = Image.FromFile("D:\\images\\gThreshold_SP15.jpg");
            double[] mask = { 255, 255, 255, 255, 255, 255, 255, 255, 255 };
            Image imEro = proc.erosion(im, mask, 3);
            imEro.Save("D:\\images\\gThreshold_SP15_Ero.jpg");
            PointPairList[] imList = proc.imageToList(im);
            PointPairList[] imEroList = proc.imageToList(imEro);
            imEroList = proc.subtractionList(imList, imEroList, im.Height, im.Width);
            imEro = proc.listToImage(imEroList, im.Height, im.Width);
            imEro.Save("D:\\images\\gThreshold_SP15_EroSub.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_SP15.jpg", "D:\\images\\gThreshold_SP15_Ero.jpg", "D:\\images\\gThreshold_SP15_EroSub.jpg" });
*/
            Image im = Image.FromFile("D:\\images\\gThreshold_SUM15.jpg");
            int[] mask = { 255, 255, 255, 255, 255, 255, 255, 255, 255 };
            Image imEro = proc.erosion(im, mask, 3);
            imEro.Save("D:\\images\\gThreshold_SUM15_Ero.jpg");
            PointPairList[] imList = proc.imageToList(im);
            PointPairList[] imEroList = proc.imageToList(imEro);
            imEroList = proc.subtractionList(imList, imEroList, im.Height, im.Width);
            imEro = proc.listToImage(imEroList, im.Height, im.Width);
            imEro.Save("D:\\images\\gThreshold_SUM15_EroSub.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_SUM15.jpg", "D:\\images\\gThreshold_SUM15_Ero.jpg", "D:\\images\\gThreshold_SUM15_EroSub.jpg" });
 /**/
            return bitmap;
        }

        public static Bitmap dilatation(Processing proc, Analysis analysis, RdWr rdwr)
        {/*
            Image im = Image.FromFile("D:\\images\\gThreshold_1.jpg");
            
            double[] mask = new double[25];
            for (int i = 0; i < 25; i++)
            {
                mask[i] = 255;
            }
            Image imDil = proc.dilatation(im, mask, 5);
            imDil.Save("D:\\images\\MODEL_Dilatation.jpg");
            PointPairList[] imList = proc.imageToList(im);
            PointPairList[] imDilList = proc.imageToList(imDil);
            imDilList = proc.subtractionList(imDilList, imList, im.Height, im.Width);
            imDil = proc.listToImage(imDilList, im.Height, im.Width);
            imDil.Save("D:\\images\\MODEL_DilSub.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_1.jpg", "D:\\images\\MODEL_Dilatation.jpg", "D:\\images\\MODEL_DilSub.jpg" });
            
            
            Image im = Image.FromFile("D:\\images\\gThreshold_gauss15.jpg");
            double[] mask = new double[25];
            for (int i = 0; i < 25; i++)
            {
                mask[i] = 255;
            }
            Image imDil = proc.dilatation(im, mask, 5);
            imDil.Save("D:\\images\\gThreshold_gauss15_Dil.jpg");
            PointPairList[] imList = proc.imageToList(im);
            PointPairList[] imDilList = proc.imageToList(imDil);
            imDilList = proc.subtractionList(imDilList, imList, im.Height, im.Width);
            imDil = proc.listToImage(imDilList, im.Height, im.Width);
            imDil.Save("D:\\images\\gThreshold_gauss15_DilSub.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_gauss15.jpg", "D:\\images\\gThreshold_gauss15_Dil.jpg", "D:\\images\\gThreshold_gauss15_DilSub.jpg" });

            
            Image im = Image.FromFile("D:\\images\\gThreshold_SP15.jpg");
            double[] mask = new double[25];
            for (int i = 0; i < 25; i++)
            {
                mask[i] = 255;
            }
            Image imDil = proc.dilatation(im, mask, 5);
            imDil.Save("D:\\images\\gThreshold_SP15_Dil.jpg");
            PointPairList[] imList = proc.imageToList(im);
            PointPairList[] imDilList = proc.imageToList(imDil);
            imDilList = proc.subtractionList(imDilList, imList, im.Height, im.Width);
            imDil = proc.listToImage(imDilList, im.Height, im.Width);
            imDil.Save("D:\\images\\gThreshold_SP15_DilSub.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_SP15.jpg", "D:\\images\\gThreshold_SP15_Dil.jpg", "D:\\images\\gThreshold_SP15_DilSub.jpg" });
*/

            Image im = Image.FromFile("D:\\images\\gThreshold_SUM15.jpg");
            int[] mask = new int[25];
            for (int i = 0; i < 25; i++)
            {
                mask[i] = 255;
            }
            Image imDil = proc.dilatation(im, mask, 5);
            imDil.Save("D:\\images\\gThreshold_SUM15_Dil.jpg");
            PointPairList[] imList = proc.imageToList(im);
            PointPairList[] imDilList = proc.imageToList(imDil);
            imDilList = proc.subtractionList(imDilList, imList, im.Height, im.Width);
            imDil = proc.listToImage(imDilList, im.Height, im.Width);
            imDil.Save("D:\\images\\gThreshold_SUM15_DilSub.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_SUM15.jpg", "D:\\images\\gThreshold_SUM15_Dil.jpg", "D:\\images\\gThreshold_SUM15_DilSub.jpg" });
/**/

            return bitmap;
        }

        public static Bitmap laplasian(Processing proc, Analysis analysis, RdWr rdwr)
        {
            Image im = Image.FromFile("D:\\MODEL.jpg");
            Image imLap = proc.laplasian(im, 3, 0.5);
            imLap.Save("D:\\images\\MODEL_Laplasian.jpg");
            /*
            PointPairList[] imGradList = proc.gradient(im, 3);
            imGradList = proc.grayScale(imGradList, im.Height, im.Width);
            Image imGrad = proc.listToImage(imGradList, im.Height, im.Width);
            imGrad.Save("D:\\images\\MODEL_Grad.jpg");*/

            Image imGauss = Image.FromFile("D:\\images\\noise15_Median.jpg");
            Image imGaussLap = proc.laplasian(imGauss, 3, 0.5);
            imGaussLap.Save("D:\\images\\noise15_Laplasian.jpg");
            /*PointPairList[] imGauss_GradList = proc.gradient(imGauss, 3);
            imGauss_GradList = proc.grayScale(imGauss_GradList, im.Height, im.Width);
            Image imGauss_Grad = proc.listToImage(imGauss_GradList, im.Height, im.Width);
            imGauss_Grad.Save("D:\\images\\noise15_Grad.jpg");*/

            Image imSP = Image.FromFile("D:\\images\\noiseSP15_Median.jpg");
            Image imSP_Lap = proc.laplasian(imSP, 3, 0.5);
            imSP_Lap.Save("D:\\images\\noiseSP15_Laplasian.jpg");
            /*PointPairList[] imSP_GradList = proc.gradient(imSP, 3);
            imSP_GradList = proc.grayScale(imSP_GradList, im.Height, im.Width);
            Image imSP_Grad = proc.listToImage(imSP_GradList, im.Height, im.Width);
            imSP_Grad.Save("D:\\images\\noiseSP15_Grad.jpg");*/

            Image imSUM = Image.FromFile("D:\\images\\noiseSUM15_Median.jpg");
            Image imSUM_Lap = proc.laplasian(imSUM, 3, 0.5);
            imSUM.Save("D:\\images\\noiseSUM15_Laplasian.jpg");
            PointPairList[] imSUM_GradList = proc.gradient(imSP, 3);
            imSUM_GradList = proc.grayScale(imSUM_GradList, im.Height, im.Width);
            Image imSUM_Grad = proc.listToImage(imSUM_GradList, im.Height, im.Width);
            imSUM_Grad.Save("D:\\images\\noiseSUM15_Grad.jpg");


            //Bitmap bitmap = CombineBitmap(new[] { "D:\\MODEL.jpg", "D:\\images\\MODEL_Laplasian.jpg" });
            //Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\noise15_Median.jpg", "D:\\images\\noise15_Laplasian.jpg" });
            //Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\noiseSP15_Median.jpg", "D:\\images\\noiseSP15_Laplasian.jpg" });
            //Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\noiseSUM15_Median.jpg", "D:\\images\\noiseSUM15_Laplasian.jpg" });
            //Bitmap bitmap = CombineBitmap(new[] { "D:\\MODEL.jpg", "D:\\images\\MODEL_Grad.jpg" });
            //Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\noise15_Median.jpg", "D:\\images\\noise15_Grad.jpg" });
            //Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\noiseSP15_Median.jpg", "D:\\images\\noiseSP15_Grad.jpg" });
            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\noiseSUM15_Median.jpg", "D:\\images\\noiseSUM15_Grad.jpg" });
            return bitmap;
        }

        public static Dictionary<PointPairList, String> histoModel (Analysis analysis, Processing proc)
        {
            Image im = Image.FromFile("D:\\MODEL.jpg");
            PointPairList l = analysis.imgHistogram(im);
            im = proc.globalThresholdConversion(im, 200);
            PointPairList[] imList = proc.imageToList(im);
            /*
            //Графики спектров для определения частоты фильтрации 15% шума
            PointPairList derString100 = proc.derivative(imList[150], 1);
            PointPairList specDerString100 = analysis.buildSpecter(derString100, 2);
            PointPairList corrString100 = analysis.buildCorrelation(derString100);
            PointPairList specCorrString100 = analysis.buildSpecter(corrString100, 2);
            for (int i = 0; i < specDerString100.Count(); i++)
            {
                specDerString100.ElementAt(i).X = specDerString100.ElementAt(i).X / im.Width;
            }
            for (int i = 0; i < specCorrString100.Count(); i++)
            {
                specCorrString100.ElementAt(i).X = specCorrString100.ElementAt(i).X / im.Width;
            }

            */
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            //list.Add(l, "");
            Image imH = Image.FromFile("D:\\images\\gThreshold_HPF.jpg");
            PointPairList lH = analysis.imgHistogram(imH);
            list.Add(lH, "");
            Image imL = Image.FromFile("D:\\images\\gThreshold_LPF.jpg");
            PointPairList lL = analysis.imgHistogram(imL);
            //list.Add(lL, "");

            //list.Add(derString100, "");
            //list.Add(specDerString100, "");
            //list.Add(corrString100, "");
            //list.Add(specCorrString100, "");

            return list;
        }

        public static Bitmap contourLPF(Processing proc, Analysis analysis, RdWr rdwr, Model model)
        {
            /*
            Image im = Image.FromFile("D:\\MODEL.jpg");
            //Пороговое преобразование
            Image imT = proc.globalThresholdConversion(im, 200);
            imT.Save("D:\\images\\gThreshold_1.jpg");
            PointPairList[] imListLPF = proc.imageToList(imT);
            PointPairList[] imTList = proc.imageToList(imT);
            //Фильтруем ФНЧ
            PointPairList lpf = proc.lpf(2, 1, 0.05);
            //по строкам
            for (int i = 0; i < imT.Height; i++)
            {
                imListLPF[i] = model.convolution(imListLPF[i], lpf);
            }
            PointPairList[] imListRotate = proc.rotateList(imListLPF, im.Height, im.Width);//переворачиваем
            //по столбцам
            for (int i = 0; i < imT.Width; i++)
            {
                imListRotate[i] = model.convolution(imListRotate[i], lpf);
            }
            imListLPF = proc.rotateList(imListRotate, im.Width, im.Height);//обратно переворачиваем
            Image imLPF = proc.listToImage(proc.grayScale(imListLPF, im.Height, im.Width), im.Height, im.Width);
            imLPF.Save("D:\\images\\gThreshold_LPF.jpg");
            //Вычитаем из порогового ФНЧ
            imTList = proc.subtractionList(imTList, imListLPF, im.Height, im.Width);
            imTList = proc.grayScale(imTList, im.Height, im.Width);
            Image imSub = proc.listToImage(imTList, im.Height, im.Width);
            //imSub = proc.globalThresholdConversion(imSub, 125);
            imSub.Save("D:\\images\\gThreshold_LPF2.jpg");

            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_1.jpg", "D:\\images\\gThreshold_LPF.jpg", "D:\\images\\gThreshold_LPF2.jpg" });
            
            //Гауссов шум
            Image im = Image.FromFile("D:\\images\\noise15.jpg");
            //im = proc.MMFilter(im, 5, 1);
            //Пороговое преобразование
            Image imT = proc.globalThresholdConversion(im, 200);
            imT.Save("D:\\images\\gThreshold_gauss15_L.jpg");
            PointPairList[] imListLPF = proc.imageToList(imT);
            PointPairList[] imTList = proc.imageToList(imT);
            //Фильтруем ФНЧ
            PointPairList lpf = proc.lpf(2, 1, 0.05);
            //по строкам
            for (int i = 0; i < imT.Height; i++)
            {
                imListLPF[i] = model.convolution(imListLPF[i], lpf);
            }
            PointPairList[] imListRotate = proc.rotateList(imListLPF, im.Height, im.Width);//переворачиваем
            //по столбцам
            for (int i = 0; i < imT.Width; i++)
            {
                imListRotate[i] = model.convolution(imListRotate[i], lpf);
            }
            imListLPF = proc.rotateList(imListRotate, im.Width, im.Height);//обратно переворачиваем
            Image imLPF = proc.listToImage(proc.grayScale(imListLPF, im.Height, im.Width), im.Height, im.Width);
            imLPF.Save("D:\\images\\gThreshold_gauss15_LPF.jpg");
            //Вычитаем из порогового ФНЧ
            imTList = proc.subtractionList(imTList, imListLPF, im.Height, im.Width);
            imTList = proc.grayScale(imTList, im.Height, im.Width);
            Image imSub = proc.listToImage(imTList, im.Height, im.Width);
            //imSub = proc.globalThresholdConversion(imSub, 125);
            imSub.Save("D:\\images\\gThreshold_gauss15_LPF2.jpg");

            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\noise15.jpg", "D:\\images\\gThreshold_gauss15_L.jpg", "D:\\images\\gThreshold_gauss15_LPF.jpg", "D:\\images\\gThreshold_gauss15_LPF2.jpg" });


            //Соль перец
            Image im = Image.FromFile("D:\\images\\noiseSP15.jpg");
            im = proc.MMFilter(im, 5, 1);
            //Пороговое преобразование
            Image imT = proc.globalThresholdConversion(im, 200);
            imT.Save("D:\\images\\gThreshold_SP15_L.jpg");
            PointPairList[] imListLPF = proc.imageToList(imT);
            PointPairList[] imTList = proc.imageToList(imT);
            //Фильтруем ФНЧ
            PointPairList lpf = proc.lpf(2, 1, 0.05);
            //по строкам
            for (int i = 0; i < imT.Height; i++)
            {
                imListLPF[i] = model.convolution(imListLPF[i], lpf);
            }
            PointPairList[] imListRotate = proc.rotateList(imListLPF, im.Height, im.Width);//переворачиваем
            //по столбцам
            for (int i = 0; i < imT.Width; i++)
            {
                imListRotate[i] = model.convolution(imListRotate[i], lpf);
            }
            imListLPF = proc.rotateList(imListRotate, im.Width, im.Height);//обратно переворачиваем
            Image imLPF = proc.listToImage(proc.grayScale(imListLPF, im.Height, im.Width), im.Height, im.Width);
            imLPF.Save("D:\\images\\gThreshold_SP15_LPF.jpg");
            //Вычитаем из порогового ФНЧ
            imTList = proc.subtractionList(imTList, imListLPF, im.Height, im.Width);
            imTList = proc.grayScale(imTList, im.Height, im.Width);
            Image imSub = proc.listToImage(imTList, im.Height, im.Width);
            //imSub = proc.globalThresholdConversion(imSub, 125);
            imSub.Save("D:\\images\\gThreshold_SP15_LPF2.jpg");

            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_SP15_L.jpg", "D:\\images\\gThreshold_SP15_LPF.jpg", "D:\\images\\gThreshold_SP15_LPF2.jpg" });
*/
            //Суммарный
            Image im = Image.FromFile("D:\\images\\noiseSUM15.jpg");
            im = proc.MMFilter(im, 5, 1);
            //Пороговое преобразование
            Image imT = proc.globalThresholdConversion(im, 210);
            imT.Save("D:\\images\\gThreshold_SUM15_L.jpg");
            PointPairList[] imListLPF = proc.imageToList(imT);
            PointPairList[] imTList = proc.imageToList(imT);
            //Фильтруем ФНЧ
            PointPairList lpf = proc.lpf(2, 1, 0.05);
            //по строкам
            for (int i = 0; i < imT.Height; i++)
            {
                imListLPF[i] = model.convolution(imListLPF[i], lpf);
            }
            PointPairList[] imListRotate = proc.rotateList(imListLPF, im.Height, im.Width);//переворачиваем
            //по столбцам
            for (int i = 0; i < imT.Width; i++)
            {
                imListRotate[i] = model.convolution(imListRotate[i], lpf);
            }
            imListLPF = proc.rotateList(imListRotate, im.Width, im.Height);//обратно переворачиваем
            Image imLPF = proc.listToImage(proc.grayScale(imListLPF, im.Height, im.Width), im.Height, im.Width);
            imLPF.Save("D:\\images\\gThreshold_SUM15_LPF.jpg");
            //Вычитаем из порогового ФНЧ
            imTList = proc.subtractionList(imTList, imListLPF, im.Height, im.Width);
            imTList = proc.grayScale(imTList, im.Height, im.Width);
            Image imSub = proc.listToImage(imTList, im.Height, im.Width);
            //imSub = proc.globalThresholdConversion(imSub, 125);
            imSub.Save("D:\\images\\gThreshold_SUM15_LPF2.jpg");

            Bitmap bitmap = CombineBitmap(new[] {"D:\\images\\gThreshold_SUM15_L.jpg", "D:\\images\\gThreshold_SUM15_LPF.jpg", "D:\\images\\gThreshold_SUM15_LPF2.jpg" });
/**/
            return bitmap;
        }

        public static Bitmap contourHPF(Processing proc, Analysis analysis, RdWr rdwr, Model model)
        {
            /*
            Image im = Image.FromFile("D:\\MODEL.jpg");
            Image imT = proc.globalThresholdConversion(im, 200);
            imT.Save("D:\\images\\gThreshold_1.jpg");
            PointPairList[] imTList = proc.imageToList(imT);
            PointPairList hpf = proc.hpf(8, 1, 0.05);
            for (int i=0; i < imT.Height; i++)
            {
                imTList[i] = model.convolution(imTList[i], hpf);
            }
            PointPairList[] imTList2 = proc.rotateList(imTList, im.Height, im.Width);
            for (int i = 0; i < imT.Width; i++)
            {
                imTList2[i] = model.convolution(imTList2[i], hpf);
            }
            imTList = proc.rotateList(imTList2, im.Width, im.Height);
            imTList = proc.grayScale(imTList, im.Height, im.Width);
            Image imHPF = proc.listToImage(imTList, im.Height, im.Width);
            imHPF.Save("D:\\images\\gThreshold_HPF.jpg");

            imHPF = proc.globalThresholdConversion(imHPF, 115);
            imHPF.Save("D:\\images\\gThreshold_HPF2.jpg");

            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_1.jpg", "D:\\images\\gThreshold_HPF.jpg", "D:\\images\\gThreshold_HPF2.jpg" });
            
            //Гауссов шум
            
            Image im = Image.FromFile("D:\\images\\noise15.jpg");
            Image imT = proc.globalThresholdConversion(im, 200);
            imT.Save("D:\\images\\gThreshold_gauss15.jpg");
            PointPairList[] imTList = proc.imageToList(imT);
            PointPairList hpf = proc.hpf(8, 1, 0.05);
            for (int i = 0; i < imT.Height; i++)
            {
                imTList[i] = model.convolution(imTList[i], hpf);
            }
            PointPairList[] imTList2 = proc.rotateList(imTList, im.Height, im.Width);
            for (int i = 0; i < imT.Width; i++)
            {
                imTList2[i] = model.convolution(imTList2[i], hpf);
            }
            imTList = proc.rotateList(imTList2, im.Width, im.Height);
            imTList = proc.grayScale(imTList, im.Height, im.Width);
            Image imHPF = proc.listToImage(imTList, im.Height, im.Width);
            imHPF.Save("D:\\images\\gThreshold_gauss15_HPF.jpg");

            imHPF = proc.globalThresholdConversion(imHPF, 160);
            imHPF.Save("D:\\images\\gThreshold_gauss15_HPF2.jpg");

            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\noise15.jpg",  "D:\\images\\gThreshold_gauss15_HPF.jpg", "D:\\images\\gThreshold_gauss15.jpg","D:\\images\\gThreshold_gauss15_HPF2.jpg" });
            
            //Соль перец
            Image im = Image.FromFile("D:\\images\\noiseSP15.jpg");
            im = proc.MMFilter(im, 5, 1);
            im.Save("D:\\images\\noiseSP15_Median.jpg");
            Image imT = proc.globalThresholdConversion(im, 200);
            imT.Save("D:\\images\\gThreshold_SP15.jpg");
            PointPairList[] imTList = proc.imageToList(imT);
            PointPairList hpf = proc.hpf(8, 1, 0.05);
            for (int i = 0; i < imT.Height; i++)
            {
                imTList[i] = model.convolution(imTList[i], hpf);
            }
            PointPairList[] imTList2 = proc.rotateList(imTList, im.Height, im.Width);
            for (int i = 0; i < imT.Width; i++)
            {
                imTList2[i] = model.convolution(imTList2[i], hpf);
            }
            imTList = proc.rotateList(imTList2, im.Width, im.Height);
            imTList = proc.grayScale(imTList, im.Height, im.Width);
            Image imHPF = proc.listToImage(imTList, im.Height, im.Width);
            imHPF.Save("D:\\images\\gThreshold_SP15_HPF.jpg");

            imHPF = proc.globalThresholdConversion(imHPF, 160);
            imHPF.Save("D:\\images\\gThreshold_SP15_HPF2.jpg");

            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\noiseSP15_Median.jpg",  "D:\\images\\gThreshold_SP15_HPF.jpg", "D:\\images\\gThreshold_SP15.jpg","D:\\images\\gThreshold_SP15_HPF2.jpg" });
            */
            //Суммарный
            Image im = Image.FromFile("D:\\images\\noiseSUM15.jpg");
            im = proc.MMFilter(im, 5, 1);
            Image imT = proc.globalThresholdConversion(im, 210);
            imT.Save("D:\\images\\gThreshold_SUM15.jpg");
            PointPairList[] imTList = proc.imageToList(imT);
            PointPairList hpf = proc.hpf(8, 1, 0.05);
            for (int i = 0; i < imT.Height; i++)
            {
                imTList[i] = model.convolution(imTList[i], hpf);
            }
            PointPairList[] imTList2 = proc.rotateList(imTList, im.Height, im.Width);
            for (int i = 0; i < imT.Width; i++)
            {
                imTList2[i] = model.convolution(imTList2[i], hpf);
            }
            imTList = proc.rotateList(imTList2, im.Width, im.Height);
            imTList = proc.grayScale(imTList, im.Height, im.Width);
            Image imHPF = proc.listToImage(imTList, im.Height, im.Width);
            imHPF.Save("D:\\images\\gThreshold_SUM15_HPF.jpg");

            imHPF = proc.globalThresholdConversion(imHPF, 140);
            imHPF.Save("D:\\images\\gThreshold_SUM15_HPF2.jpg");

            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\gThreshold_SUM15.jpg", "D:\\images\\gThreshold_SUM15_HPF.jpg", "D:\\images\\gThreshold_SUM15_HPF2.jpg" });
            /**/
            return bitmap;
        }

        public static Bitmap blur(Processing proc, Analysis analysis, RdWr rdwr)
        {
            int h = 221, w = 307;
            PointPairList[] listFile = rdwr.readFileArray("D:\\blur.dat", h, w);//g
            PointPairList[] listFileN = rdwr.readFileArray("D:\\blur_N.dat", h, w);
            PointPairList listKern = rdwr.readFile("D:\\kern.dat", 76);//h
            Image im = proc.listToImage(listFile, h, w);
            Image imN = proc.listToImage(listFileN, h, w);
            im.Save("D:\\images\\blur.jpg");
            imN.Save("D:\\images\\blurN.jpg");

            //Идеальный
            
            for (int i = listKern.Count(); i < w; i++)//дополнили нулями h
            {
                listKern.Add(i, 0);
            }
            //без шумов
            PointPairList[] listX = new PointPairList[h];
            for (int i=0; i<h; i++)
            {
                listX[i] = new PointPairList();
                listX[i]= analysis.divComplexList(listFile[i], listKern);
                listX[i] = analysis.getComplex(listX[i]);
            }
            listX = proc.grayScale(listX, h, w);
            Image imX = proc.listToImage(listX, h, w);
            imX.Save("D:\\images\\blur_Ideal.jpg");
            /**///с шумом
            PointPairList[] listXN = new PointPairList[h];
            for (int i = 0; i < h; i++)
            {
                listXN[i] = new PointPairList();
                listXN[i] = analysis.divComplexList(listFileN[i], listKern);
                listXN[i] = analysis.getComplex(listXN[i]);
            }
            listXN = proc.grayScale(listXN, h, w);
            Image imXN = proc.listToImage(listXN, h, w);
            imXN.Save("D:\\images\\blurN_Ideal.jpg");

            //Оптимальный
            
            //без шума
            /*
            PointPairList[] listX = new PointPairList[h];
            for (int i = 0; i < h; i++)
            {
                listX[i] = new PointPairList();
                listX[i] = analysis.divOptimalComplexList(listFile[i], listKern, 0.0001);
                listX[i] = analysis.getComplex(listX[i]);
            }
            listX = proc.grayScale(listX, h, w);
            Image imX = proc.listToImage(listX, h, w);
            imX.Save("D:\\images\\blur_Opt.jpg");

            //с шумом
            PointPairList[] listXN = new PointPairList[h];
            for (int i = 0; i < h; i++)
            {
                listXN[i] = new PointPairList();
                listXN[i] = analysis.divOptimalComplexList(listFileN[i], listKern, 3);
                listXN[i] = analysis.getComplex(listXN[i]);
            }
            listXN = proc.grayScale(listXN, h, w);
            Image imXN = proc.listToImage(listXN, h, w);
            imXN.Save("D:\\images\\blurN_Opt.jpg");
*/
            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\blur.jpg", "D:\\images\\blur_Ideal.jpg", "D:\\images\\blurN.jpg", "D:\\images\\blurN_Ideal.jpg" });
            //Bitmap bitmap = CombineBitmap(new[] {  "D:\\images\\blurN_Opt.jpg" });
            return bitmap;
        }

        public static Bitmap MMFilter(Processing proc)
        {/*
            //Гауссов шум
            Image im1 = Image.FromFile("D:\\images\\noise1.jpg");
            Image im5 = Image.FromFile("D:\\images\\noise5.jpg");
            Image im15 = Image.FromFile("D:\\images\\noise15.jpg");
            
            //Соль перец шум
            Image im1 = Image.FromFile("D:\\images\\noiseSP1.jpg");
            Image im5 = Image.FromFile("D:\\images\\noiseSP5.jpg");
            Image im15 = Image.FromFile("D:\\images\\noiseSP15.jpg");
            */
            //Суммарный шум
            Image im1 = Image.FromFile("D:\\images\\noiseSUM1.jpg");
            Image im5 = Image.FromFile("D:\\images\\noiseSUM5.jpg");
            Image im15 = Image.FromFile("D:\\images\\noiseSUM15.jpg");
            /**/

            Image meanFilter1 = proc.MMFilter(im1,5,0);
            Image meanFilter5 = proc.MMFilter(im5,5,0);
            Image meanFilter15 = proc.MMFilter(im15,5,0);

            meanFilter1.Save("D:\\images\\noise1_Mean.jpg");
            meanFilter5.Save("D:\\images\\noise5_Mean.jpg");
            meanFilter15.Save("D:\\images\\noise15_Mean.jpg");
            
            Image medianFilter1 = proc.MMFilter(im1,5,1);
            Image medianFilter5 = proc.MMFilter(im5,5,1);
            Image medianFilter15 = proc.MMFilter(im15,5,1);
            


            medianFilter1.Save("D:\\images\\noise1_Median.jpg");
            medianFilter5.Save("D:\\images\\noise5_Median.jpg");
            medianFilter15.Save("D:\\images\\noise15_Median.jpg");

            Bitmap bitmap = CombineBitmap(new[] { "D:\\images\\noiseSUM15.jpg", "D:\\images\\noise15_Mean.jpg", "D:\\images\\noise15_Median.jpg" });
            return bitmap;
        }

        public static Bitmap SUMNoise(Processing proc, Analysis analysis)
        {
            //Bitmap bitmap = CombineBitmap(new[] { "L:\\MODEL.jpg", "L:\\images\\noiseSUM5.jpg", "L:\\images\\noiseSUM1.jpg", "L:\\images\\noiseSUM15.jpg" });
            Bitmap bitmap = CombineBitmap(new[] {  "L:\\images\\noiseSUM15.jpg", "L:\\images\\noiseSUM15_LPF.jpg" });
            return bitmap;
        }

        public static Bitmap SPNoise(Processing proc, Analysis analysis)
        {
            //Bitmap bitmap = CombineBitmap(new[] { "L:\\MODEL.jpg", "L:\\images\\noiseSP5.jpg", "L:\\images\\noiseSP1.jpg", "L:\\images\\noiseSP15.jpg" });
            Bitmap bitmap = CombineBitmap(new[] { "L:\\images\\noiseSP15.jpg", "L:\\images\\noiseSPLPF15.jpg" });
            return bitmap;
        }

        public static Bitmap normNoise(Processing proc, Analysis analysis)
        {
            //Bitmap bitmap = CombineBitmap(new[] { "L:\\MODEL.jpg" , "L:\\images\\noise5.jpg" , "L:\\images\\noise1.jpg", "L:\\images\\noise15.jpg" });
            Bitmap bitmap = CombineBitmap(new[] { "L:\\images\\noise15.jpg", "L:\\images\\noiseNormLPF15.jpg" });
            return bitmap;
        }

        public static Dictionary<PointPairList, String> noiseImg(Model model, Analysis analysis, Processing proc)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Image im = Image.FromFile("D:\\MODEL.jpg");
            PointPairList[] imgList = proc.imageToList(im);
            /*
            //Гауссов шум 1%
            PointPairList[] imList1 = proc.normalNoise(imgList, im.Width, im.Height, 0.01);
            imList1 = proc.grayScale(imList1, im.Height, im.Width);
            Image im1 = proc.listToImage(imList1, im.Height, im.Width);
            im1.Save("L:\\images\\noise1.jpg");

            //Гауссов шум 5%
            PointPairList[] imList5 = proc.normalNoise(imgList, im.Width, im.Height, 0.05);
            imList5 = proc.grayScale(imList5, im.Height, im.Width);
            Image im5 = proc.listToImage(imList5, im.Height, im.Width);
            im5.Save("L:\\images\\noise5.jpg");
            */
/*            //Гауссов шум 15%
            PointPairList[] imList15 = proc.normalNoise(imgList, im.Width, im.Height, 0.15);
            imList15 = proc.grayScale(imList15, im.Height, im.Width);
            Image im15 = proc.listToImage(imList15, im.Height, im.Width);
            im15.Save("D:\\images\\noise15.jpg");

            //Графики спектров для определения частоты фильтрации 15% шума
            PointPairList derString100 = proc.derivative(imList15[100], 1);
            PointPairList specDerString100 = analysis.buildSpecter(derString100, 2);
            PointPairList corrString100 = analysis.buildCorrelation(derString100);
            PointPairList specCorrString100 = analysis.buildSpecter(corrString100, 2);
            for (int i = 0; i < specDerString100.Count(); i++)
            {
                specDerString100.ElementAt(i).X = specDerString100.ElementAt(i).X / im.Width;
            }
            for (int i=0; i < specCorrString100.Count(); i++)
            {
                specCorrString100.ElementAt(i).X = specCorrString100.ElementAt(i).X / im.Width;
            }

            //Фильтр низких частот
            PointPairList listFilter = proc.lpf(32, 1, 0.1);
            for (int i = 0; i < im.Height; i++)
            {
                imList15[i] = model.convolution(imList15[i], listFilter);
            }
            imList15 = proc.grayScale(imList15, im.Height, im.Width);
            Image imLPF15 = proc.listToImage(imList15, im.Height, im.Width);
            imLPF15.Save("D:\\images\\noiseNormLPF15.jpg");


 */           
/*
            //Соль и перец 1%
            PointPairList[] imListSP1 = proc.saltPepperNoise(imgList, im.Width, im.Height, 0.01);
            Image imSP1 = proc.listToImage(imListSP1, im.Height, im.Width);
            imSP1.Save("L:\\images\\noiseSP1.jpg");

            //Соль и перец 5%
            PointPairList[] imListSP5 = proc.saltPepperNoise(imgList, im.Width, im.Height, 0.05);
            Image imSP5 = proc.listToImage(imListSP5, im.Height, im.Width);
            imSP5.Save("L:\\images\\noiseSP5.jpg");

            //Соль и перец 15%
            PointPairList[] imListSP15 = proc.saltPepperNoise(imgList, im.Width, im.Height, 0.15);
            Image imSP15 = proc.listToImage(imListSP15, im.Height, im.Width);
            imSP15.Save("D:\\images\\noiseSP15.jpg");

            //Графики спектров для определения частоты фильтрации 15% шума
            PointPairList derString100 = proc.derivative(imListSP15[100], 1);
            PointPairList specDerString100 = analysis.buildSpecter(derString100, 2);
            PointPairList corrString100 = analysis.buildCorrelation(derString100);
            PointPairList specCorrString100 = analysis.buildSpecter(corrString100, 2);
            for (int i = 0; i < specDerString100.Count(); i++)
            {
                specDerString100.ElementAt(i).X = specDerString100.ElementAt(i).X / im.Width;
            }
            for (int i=0; i < specCorrString100.Count(); i++)
            {
                specCorrString100.ElementAt(i).X = specCorrString100.ElementAt(i).X / im.Width;
            }

            //Фильтр низких частот
            PointPairList listFilter = proc.lpf(32, 1, 0.1);
            for (int i = 0; i < im.Height; i++)
            {
                imListSP15[i] = model.convolution(imListSP15[i], listFilter);
            }
            imListSP15 = proc.grayScale(imListSP15, im.Height, im.Width);
            Image imLPF15 = proc.listToImage(imListSP15, im.Height, im.Width);
            imLPF15.Save("D:\\images\\noiseSPLPF15.jpg");
*/

/*
            //Суммарный шум 1%
            PointPairList[] imListSUM1 = proc.summaryNoise(imgList, im.Width, im.Height, 0.01);
            imListSUM1 = proc.grayScale(imListSUM1, im.Height, im.Width);
            Image imSUM1 = proc.listToImage(imListSUM1, im.Height, im.Width);
            imSUM1.Save("L:\\images\\noiseSUM1.jpg");

            //Суммарный шум 5%
            PointPairList[] imListSUM5 = proc.summaryNoise(imgList, im.Width, im.Height, 0.05);
            imListSUM5 = proc.grayScale(imListSUM5, im.Height, im.Width);
            Image imSUM5 = proc.listToImage(imListSUM5, im.Height, im.Width);
            imSUM5.Save("L:\\images\\noiseSUM5.jpg");
*/
            //Суммарный шум 15%
            PointPairList[] imListSUM15 = proc.summaryNoise(imgList, im.Width, im.Height, 0.15);
            imListSUM15 = proc.grayScale(imListSUM15, im.Height, im.Width);
            Image imSUM15 = proc.listToImage(imListSUM15, im.Height, im.Width);
            imSUM15.Save("D:\\images\\noiseSUM15.jpg");

            //Графики спектров для определения частоты фильтрации 15% шума
            PointPairList derString100 = proc.derivative(imListSUM15[100], 1);
            PointPairList specDerString100 = analysis.buildSpecter(derString100, 2);
            PointPairList corrString100 = analysis.buildCorrelation(derString100);
            PointPairList specCorrString100 = analysis.buildSpecter(corrString100, 2);
            for (int i = 0; i < specDerString100.Count(); i++)
            {
                specDerString100.ElementAt(i).X = specDerString100.ElementAt(i).X / im.Width;
            }
            for (int i = 0; i < specCorrString100.Count(); i++)
            {
                specCorrString100.ElementAt(i).X = specCorrString100.ElementAt(i).X / im.Width;
            }

            //Фильтр низких частот
            PointPairList listFilter = proc.lpf(32, 1, 0.1);
            for (int i = 0; i < im.Height; i++)
            {
                imListSUM15[i] = model.convolution(imListSUM15[i], listFilter);
            }
            imListSUM15 = proc.grayScale(imListSUM15, im.Height, im.Width);
            Image imLPF15 = proc.listToImage(imListSUM15, im.Height, im.Width);
            imLPF15.Save("D:\\images\\noiseSUM15_LPF.jpg");

            //Спектр после фильтрации
            PointPairList derStr100_15 = proc.derivative(imListSUM15[100], 1);
            PointPairList specDer100_15 = analysis.buildSpecter(derStr100_15, 2);
            PointPairList corrStr100_15 = analysis.buildCorrelation(imListSUM15[100]);
            PointPairList specCorr100_15 = analysis.buildSpecter(corrStr100_15, 2);
            for (int i = 0; i < specDer100_15.Count(); i++)
            {
                specDer100_15.ElementAt(i).X = specDer100_15.ElementAt(i).X / im.Width;
            }
            for (int i = 0; i < specCorr100_15.Count(); i++)
            {
                specCorr100_15.ElementAt(i).X = specCorr100_15.ElementAt(i).X / im.Width;
            }
            list.Add(derStr100_15, "x100'");
            list.Add(specDer100_15, "| x100' |");
            list.Add(corrStr100_15, "R(x100'x100')");
            list.Add(specCorr100_15, "| R(x100'x100') |");

            //list.Add(analysis.imgHistogram(im),"Гистограмма исходного изображения");
            /*list.Add(analysis.imgHistogram(im1), "1% зашумления");
            list.Add(analysis.imgHistogram(im5), "5% зашумления");
            list.Add(analysis.imgHistogram(im15), "15% зашумления");

            list.Add(analysis.imgHistogram(imSP1), "1% зашумления");
            list.Add(analysis.imgHistogram(imSP5), "5% зашумления");
            list.Add(analysis.imgHistogram(imSP15), "15% зашумления");

            list.Add(analysis.imgHistogram(imSUM1), "1% зашумления");
            list.Add(analysis.imgHistogram(imSUM5), "5% зашумления");
            list.Add(analysis.imgHistogram(imSUM15), "15% зашумления");
            list.Add(derString100, "x100'");
            list.Add(specDerString100, "| x100' |");
            list.Add(corrString100, "R(x100'x100')");
            list.Add(specCorrString100, "| R(x100' x100') |");*/
            return list;
        }

        public static Dictionary<PointPairList, String> xray(Model model, Analysis analysis, Processing proc)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();

            Image im = Image.FromFile("L:\\xray.jpg");
            PointPairList listIm = analysis.imgCDF(im);
            Image imCDF = proc.corrCDF(im, listIm);
            imCDF.Save("L:\\xrayCDF.jpg");

            PointPairList[] imgList = proc.imageToList(imCDF);
            PointPairList[] imgListFilter = new PointPairList[im.Height];
            for (int i = 0; i < im.Height; i++)
            {
                imgListFilter[i] = new PointPairList();
            }

            PointPairList derString100 = proc.derivative(imgList[100], 1);
            PointPairList specDerString100 = analysis.buildSpecter(derString100, 2);
            PointPairList corrString100 = analysis.buildCorrelation(derString100);
            PointPairList specCorrString100 = analysis.buildSpecter(corrString100, 2);
            for (int i = 0; i < specDerString100.Count(); i++)
            {
                specDerString100.ElementAt(i).X = specDerString100.ElementAt(i).X / im.Width;
            }
            for (int i=0; i < specCorrString100.Count(); i++)
            {
                specCorrString100.ElementAt(i).X = specCorrString100.ElementAt(i).X / im.Width;
            }
/*
            PointPairList derString10 = proc.derivative(imgList[10], 1);
            PointPairList corrString10 = analysis.buildCorrelation(derString10);
            PointPairList corrStr1_10 = analysis.buildCorrelation(derString1, derString10);
            PointPairList specCorr1_10 = analysis.buildSpecter(corrStr1_10, 2);

 */
            PointPairList listFilter = proc.bsf(32, 1, 0.2,0.34);
            for (int i=0; i < im.Height; i++)
            {
                imgListFilter[i] = model.convolution(imgList[i], listFilter);
            }
            PointPairList[] imgListScale = new PointPairList[im.Height];
            for (int i = 0; i < im.Height; i++)
            {
                imgListScale[i] = new PointPairList();
            }
            imgListScale = proc.grayScale(imgListFilter, im.Height, im.Width);
            Image newIm = proc.listToImage(imgListScale, im.Height, im.Width);
            newIm.Save("L:\\xray_filter.jpg");

            //list.Add(imgList[100], "x100");
            list.Add(derString100, "x100'");
            list.Add(specDerString100, "| x100' |");
            list.Add(corrString100, "R(x100'x100')");
            list.Add(specCorrString100, "| R(x100' x100') |");

            return list;
        }

        public static Bitmap xrayCDF(Processing proc, Analysis analysis)
        {
            Image im = Image.FromFile("L:\\xray.jpg");
            PointPairList listIm = analysis.imgCDF(im);
            Image imCDF = proc.corrCDF(im, listIm);
            imCDF.Save("L:\\xrayCDF.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "L:\\xray.jpg", "L:\\xrayCDF.jpg" });

            return bitmap;
        }

        public static Bitmap correctionCDF(Processing proc, Analysis analysis)
        {
            Image im = Image.FromFile("L:\\HollywoodLC.jpg");
            PointPairList list = analysis.imgCDF(im);
            Image imCDF = proc.corrCDF(im, list);
            imCDF.Save("L:\\CDF.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "L:\\HollywoodLC.jpg", "L:\\CDF.jpg" });

            return bitmap;
        }

        public static Dictionary<PointPairList, String> imageHistogram(Model model, Analysis analysis, Processing proc)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Image im = Image.FromFile("L:\\HollywoodLC.jpg");

            PointPairList list1 = analysis.imgHistogram(im);
            PointPairList list2 = analysis.imgCDF(im);
            list.Add(list1, "histogram");
            list.Add(list2, "CDF");

            return list;
        }
        public static Bitmap contrast(Processing proc)
        {
            Image im1 = Image.FromFile("L:\\image1.jpg");
            Image im2 = Image.FromFile("L:\\image2.jpg");

            //Негатив 
            /*
            Image im1n = proc.negative(im1);
            im1n.Save("L:\\im1_Neg.jpg");
            Image im2n = proc.negative(im2);
            im2n.Save("L:\\im2_Neg.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "L:\\image1.jpg", "L:\\im1_Neg.jpg", "L:\\image2.jpg" , "L:\\im2_Neg.jpg" });
            */
            //Гамма
           
            Image im1g = proc.gamma(im1,1,0.95);
            im1g.Save("L:\\images\\im1_Gamma.jpg");
            Image im2g = proc.gamma(im2,1,0.95);
            im2g.Save("L:\\images\\im2_Gamma.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "L:\\image1.jpg", "L:\\images\\im1_Gamma.jpg", "L:\\image2.jpg", "L:\\images\\im2_Gamma.jpg" });

/*
            //Логарифм
            
            Image im1l = proc.logarifm(im1,40);
            im1l.Save("L:\\images\\im1_Log.jpg");
            Image im2l = proc.logarifm(im2,40);
            im2l.Save("L:\\images\\im2_Log.jpg");
            Bitmap bitmap = CombineBitmap(new[] { "L:\\image1.jpg", "L:\\images\\im1_Log.jpg", "L:\\image2.jpg", "L:\\images\\im2_Log.jpg" });
              */

            return bitmap;
        }

        public static Bitmap resizeIm(Processing proc)
        {
            Image im1 = Image.FromFile("D:\\grace.jpg");
            im1 = proc.bilinearInterp(im1, 0.77, 0.77);
            im1.Save("D:\\graceBI13.jpg");

            /*im1 = proc.nearestNeighbors(im1, 2, 2);
            im1.Save("L:\\graceNN2.jpg");*/
            /*
            im1 = proc.nearestNeighbors(im1, 2.7, 2.7);
            im1.Save("L:\\graceNN27.jpg");
            
            im1 = proc.nearestNeighbors(im1, 0.77, 0.77);
            im1.Save("D:\\graceNN13.jpg");
            */
            /*
            
            im1 = proc.bilinearInterp(im1, 2, 2);
            im1.Save("L:\\graceBI2.jpg");

            im1 = proc.bilinearInterp(im1, 2.7, 2.7);
            im1.Save("L:\\graceBI27.jpg");
            
            
            
            */
            //Bitmap bitmap = CombineBitmap(new[] { "E:\\grace.jpg","E:\\images\\graceNN13.jpg", "E:\\images\\graceBI13.jpg" });
            Bitmap bitmap = CombineBitmap(new[] { "D:\\grace.jpg", "D:\\graceBI13.jpg" });
            return bitmap;
        }

        public static Bitmap CombineBitmap(IEnumerable<string> files)
        {
            //read all images into memory
            List<Bitmap> images = new List<Bitmap>();
            Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (string image in files)
                {
                    // create a Bitmap from the file and add it to the list
                    Bitmap bitmap = new Bitmap(image);

                    // update the size of the final bitmap
                    width += bitmap.Width;
                    height += bitmap.Height ;

                    images.Add(bitmap);
                }

                // create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                // get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    // set background color
                    g.Clear(Color.Transparent);

                    // go through each image and draw it on the final image
                    int i = 0, x=0, y=0;
                    foreach (Bitmap image in images)
                    {
                        if (i ==0)
                        {
                            g.DrawImage(image, new Rectangle(x, y, image.Width, image.Height));
                            x = image.Width;
                            y = image.Height;
                        }else
                        if (i == 1)
                        {
                            g.DrawImage(image, new Rectangle(x, 0, image.Width, image.Height));
                            
                        }
                        else
                        if (i == 2)
                        {
                            g.DrawImage(image, new Rectangle(0, y, image.Width, image.Height));

                        }
                        else
                        if (i==3)
                        {
                            g.DrawImage(image, new Rectangle(x, y, image.Width, image.Height));
                        }
                        
                        i++;
                    }
                }

                return finalImage;
            }
            catch (Exception)
            {
                if (finalImage != null) finalImage.Dispose();
                throw;
            }
            finally
            {
                // clean up memory
                foreach (Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }

        public static Dictionary<PointPairList, String> furieObr(Model model, Analysis analysis, Processing proc)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            int N = 1000, M = 200;
            PointPairList list1 = model.buildSinCardio(2, 0.005, 10, M, 0.2);
            PointPairList list2 = new PointPairList();
            for (int i = 0; i < N; i++)
            {
                list2.Add(i, 0);
                if (i % 250 == 0) list2.Add(i, 120);
            }
            PointPairList list3 = model.convolution(list2, list1);

            //H
            for (int i = M; i < N; i++)
            {
                list1.Add(i, 0);
            }

            //X
            PointPairList list6 = analysis.divComplexList(list3, list1);
            list6 = analysis.getComplex(list6);

            list.Add(list1, "h");
            list.Add(list3, "y");
            list.Add(list6, "x");

            return list;
        }

        public static Dictionary<PointPairList, String> zachet(Model model, Analysis analysis, Processing proc, PointPairList listFile)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            list.Add(listFile, "");
            PointPairList list1 = proc.antiTrend(listFile, 100);
            //list.Add(list1, "anti trend");
            double min = 0, max = 0;
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1.ElementAt(i).Y > max) max = list1.ElementAt(i).Y;
                if (list1.ElementAt(i).Y < min) min = list1.ElementAt(i).Y;
            }
            list1 = proc.antiSpike(list1, -500, 500);
            list.Add(list1, "anti spike+anti trend");
            list.Add(analysis.buildCorrelation(list1), "correlation");
            list.Add(analysis.buildHistogram(list1, 100), "histogram");
            list.Add(analysis.buildSpecter(list1, 2), "specter");
            double dt = (double)1 / list1.Count();
            PointPairList list2 = proc.bsf(128, dt, 60, 70);
            PointPairList list3 = proc.bsf(128, dt, 180, 220);
            list1 = model.convolution(list1, list2);
            list1 = model.convolution(list1, list3);
            list.Add(list1, "");
            list.Add(analysis.buildSpecter(list1, 2), "specter");
            return list;
        }

        public static Dictionary<PointPairList, String> courseWork(Model model, Analysis analysis, Processing proc)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();

            //информационный сигнал
            WavFile wavV = new WavFile("D:\\course.wav");
            PointPairList listV = wavV.toList();
            listV = model.cropList(listV, 15000, 50000);
            /*wavV.listToWav(listV, "()");
            wavV = new WavFile("D:\\course().wav");
            listV = wavV.toList();*/
            //list.Add(listV, "Осциллограмма");
            double df = (double)wavV.dwSamplesPerSec / (double)listV.Count;
            //list.Add(analysis.buildSpecter(listV, 2, df), "Спектр");

            //шум
            WavFile wavN = new WavFile("D:\\noise.wav");
            PointPairList list0 = wavN.toList();
            list0 = model.cropList(list0, 0, 35000);
            //list.Add(list0, "Осциллограмма");
            //list.Add(analysis.buildSpecter(list0, 2, df), "Спектр");

            //наложение шума на инф сигнал
            PointPairList list1 = model.addLists(listV, list0);
            //list.Add(list1, "Осциллограмма");
            //list.Add(analysis.buildSpecter(list1, 2, df), "Спектр");
            wavV.listToWav(list1, "(add)");

            WavFile wav1 = new WavFile("D:\\course(add).wav");
            PointPairList listt = wav1.toList();
            list.Add(listt, "Осциллограмма");
            wavV.playWav();
            /*
            Dictionary<int, PointPairList> listOfLists = new Dictionary<int, PointPairList>();
            PointPairList list11 = new PointPairList();
            for (int i=0; i < 6; i++)
            {
                list11= model.cropList(list1, i*5000, i*5000+5000);
                listOfLists.Add(i, list11);
            }
            df = (double)wavV.dwSamplesPerSec / (double)(listOfLists.ElementAt(2).Value.Count());
            
            //list.Add(list11, "");
            list.Add(analysis.buildSpecter(listOfLists.ElementAt(2).Value, 2,df), "");*/
            //list.Add(analysis.buildSpecter(listOfLists.ElementAt(0).Value,2, wav.dwSamplesPerSec), "");
            /*list.Add(analysis.buildSpecter(list12, 2), "");*/
            /*list.Add(analysis.buildSpecter(list13, 2), "");
            list.Add(analysis.buildSpecter(list14, 2), "");
            list.Add(analysis.buildSpecter(list15, 2), "");
            list.Add(analysis.buildSpecter(list16, 2), "");*/

            /*
            //фильтрация
            double dt = (double)1 / wavV.dwSamplesPerSec;
            Console.WriteLine(dt);
            PointPairList list2 = proc.bsf(256, dt, 150, 350);
            list1 = model.convolution(list1, list2);
            
            for (int i=0; i < 6; i++)
            {
                list11 = listOfLists.ElementAt(i).Value;
                list11= model.convolution(list11, list2);
                listOfLists.Remove(i);
                listOfLists.Add(i, list11);
            }
            
            list1 = model.stickList(listOfLists);
            */
            //Console.WriteLine(df);
            //list.Add(analysis.buildSpecter(listOfLists.ElementAt(2).Value, 2, df), "");

            //list.Add(analysis.buildSpecter(list1, 2, df), "Спектр");
            //list.Add(list1, "осциллограмма");
            //wavV.listToWav(list1, "(bsf150_350)");

            return list;
        }

        public static Dictionary<PointPairList, String> wav(Model model, Analysis analysis, Processing proc)
        {
            WavFile wav = new WavFile("D:\\ma.wav");
            /*wav.playWav();
            wav.writeMultiplyWav(2);
            
            WavFile wav2 = new WavFile("E:\\ma(+).wav");
            Thread.Sleep(5000);
            wav2.playWav();*/

            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            //файл
            PointPairList list1 = wav.toList(2000, 18000);
            //list.Add(list1, "осциллограмма");
            list.Add(analysis.buildSpecter(list1, 2), "спектр");
            /*
            //первая частота 40
            PointPairList list2 = proc.lpf(64, 0.0001, 40);
            PointPairList list3 = model.convolution(list1, list2);
            //list.Add(list2, "");
            //list.Add(list3, "LPF*WAV");
            //list.Add(analysis.buildSpecter(list3, 2), "|LPF*WAV|");
            wav.listToWav(list3, "(LPF40)");
            WavFile wav1 = new WavFile("D:\\ma(LPF40).wav");
            //PointPairList list4 = wav1.toList(100, 18000);
            //list.Add(list4, "");
            //list.Add(analysis.buildSpecter(list4, 2), "");
            wav1.playWav();
            */
            //последняя частота 425
            PointPairList list5 = proc.hpf(64, 0.0001, 900);
            PointPairList list6 = model.convolution(list1, list5);
            //list.Add(list5, "");
            list.Add(list6, "HPF*WAV");
            list.Add(analysis.buildSpecter(list6, 2), "|HPF*WAV|");
            wav.listToWav(list6, "(HPF900)");
            WavFile wav2 = new WavFile("D:\\ma(HPF900).wav");
            PointPairList list7 = wav2.toList(100, 18000);
            list.Add(list7, "");
            list.Add(analysis.buildSpecter(list7, 2), "");



            return list;
        }

        public static Dictionary<PointPairList, String> wavMy(Model model, Analysis analysis, Processing proc)
        {
            WavFile wav = new WavFile("D:\\mv.wav");
            //wav.playWav();
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            //файл
            PointPairList list1 = wav.toList();
            wav.playWav();

            list.Add(list1, "осциллограмма");
            list.Add(analysis.buildSpecter(list1, 2), "спектр");
            wav.listToWav(list1, "_");

            //первая частота 110
            PointPairList list2 = proc.lpf(64, 0.0001, 110);
            PointPairList list3 = model.convolution(list1, list2);
            //list.Add(list2, "");
            //list.Add(list3, "LPF(70)*WAV");
            list.Add(analysis.buildSpecter(list3, 2), "|LPF(110)*WAV|");
            wav.listToWav(list3, "(LPF110)");
            //WavFile wav1 = new WavFile("D:\\mv(LPF70).wav");
            //PointPairList list4 = wav1.toList(100, 10000);
            //list.Add(list4, "");
            //list.Add(analysis.buildSpecter(list4, 2), "");
            //wav1.playWav();
            /*
            //последняя частота
            PointPairList list5 = proc.lpf(128, 0.0001, 3730);
            PointPairList list6 = model.convolution(list1, list5);
            //list.Add(list5, "");
            //list.Add(list6, "HPF(900)*WAV");
            list.Add(analysis.buildSpecter(list6, 2), "|HPF(900)*WAV|");
            wav.listToWav(list6, "(HPF3730)");
            //WavFile wav2 = new WavFile("D:\\ma(HPF900).wav");
            //PointPairList list7 = wav2.toList(100, 18000);
            //list.Add(list7, "");
            //list.Add(analysis.buildSpecter(list7, 2), "");
            */


            return list;
        }

        public static Dictionary<PointPairList, String> trend(Model model)
        {
            int n = 5;
            double alpha = Math.Pow(10, -3) * n, beta = 1000;

            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            list.Add(model.trend(5, 100, true, 1000), "Trend #1");
            list.Add(model.trend(-5, 100, true, 1000), "Trend #2");
            list.Add(model.trend(alpha, 1, false, 1000), "Trend #3");
            list.Add(model.trend(alpha, 1, false, 1000), "Trend #4");
            list.Add(model.trendComp(5, alpha, 100, beta, 0, 1000), "Composite");
            return list;
        }

        public static Dictionary<PointPairList, String> trendNorm(Model model, RdWr rdwr)
        {
            int n = 5;
            double alpha = Math.Pow(10, -3) * n;

            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            list.Add(rdwr.norm(model.trend(5, 100, true, 1000), 1), "Trend #1");
            list.Add(rdwr.norm(model.trend(-5, 100, true, 1000), 1), "Trend #2");
            list.Add(rdwr.norm(model.trend(alpha, 1, false, 1000), 1), "Trend #3");
            list.Add(rdwr.norm(model.trend(alpha, 1, false, 1000), 1), "Trend #4");
            return list;
        }

        public static Dictionary<PointPairList, String> random(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            list.Add(model.buildRandom(-1, 1, 1000), "Random");
            list.Add(model.buildSelfRandom(1, 1000), "Self Random");
            list.Add(model.addSpikes(model.buildRandom(-1, 1, 1000)), "Spikes");
            list.Add(model.shift(model.buildRandom(-1, 1, 1000), 100, 300, 5), "Shift");
            return list;
        }

        public static Dictionary<PointPairList, String> stat(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            list.Add(analysis.meansOfIntervals(model.buildRandom(-100, 100, 1000), 10), "Means for random");
            list.Add(analysis.dispOfIntervals(model.buildRandom(-100, 100, 1000), 10), "Dispersions for random");
            list.Add(analysis.meansOfIntervals(model.buildSelfRandom(100, 1000), 10), "Means for self random");
            list.Add(analysis.dispOfIntervals(model.buildSelfRandom(100, 1000), 10), "Dispersions for self random");
            list.Add(analysis.meansOfIntervals(model.trend(0.5, 100, true, 1000), 10), "Means for trend");
            list.Add(analysis.dispOfIntervals(model.trend(0.5, 100, true, 1000), 10), "Dispersions for trend");
            return list;
        }

        public static Dictionary<PointPairList, String> addMulti(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            PointPairList list1 = model.addLists(model.trend(-0.5, 1000, true, 1000), model.buildRandom(-100, 100, 1000));
            list.Add(list1, "Trend+Random");
            list.Add(model.multiLists(model.trend(-0.5, 1000, true, 1000), model.buildRandom(-100, 100, 1000)), "Trend*Random");
            list.Add(model.addSpikes(list1), "Trend+Random+Spikes");

            return list;
        }

        public static Dictionary<PointPairList, String> multiChannel(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();

            PointPairList list1 = model.buildRandom(-100, 100, 1000);
            list.Add(list1, "Random");
            Console.WriteLine("Средняя дисперсия 1 реализации " + analysis.calcDisp(list1));

            PointPairList list2 = model.multiSumRandom(1000, 10, -100, 100);
            list.Add(list2, "10 Randoms");
            Console.WriteLine("Средняя дисперсия 10 реализаций " + analysis.calcDisp(list2));

            PointPairList list3 = model.multiSumRandom(1000, 100, -100, 100);
            list.Add(list3, "100 Randoms");
            Console.WriteLine("Средняя дисперсия 100 реализаций " + analysis.calcDisp(list3));

            return list;
        }

        public static Dictionary<PointPairList, String> histogram(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            PointPairList list1 = model.buildRandom(-100, 100, 1000);
            list.Add(list1, "Random");
            PointPairList list2 = model.buildSelfRandom(100, 1000);
            list.Add(list2, "SelfRandom");
            list.Add(analysis.buildHistogram(list1, 40), "Histogram of Random");
            list.Add(analysis.buildHistogram(list2, 40), "Histogram of Self Random");
            return list;
        }

        public static Dictionary<PointPairList, String> correlation(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            PointPairList list1 = model.buildRandom(-100, 100, 1000);
            list.Add(analysis.buildCorrelation(list1), "Correlation of Random");
            PointPairList list2 = model.buildSelfRandom(100, 1000);
            list.Add(analysis.buildCorrelation(list1, list2), "Cross Correlation");

            return list;
        }

        public static Dictionary<PointPairList, String> sinDiffFreq(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            list.Add(model.buildSin(100, 0.001, 11, 1000), "11");
            list.Add(model.buildSin(100, 0.001, 110, 1000), "110");
            list.Add(model.buildSin(100, 0.001, 250, 1000), "250");
            list.Add(model.buildSin(100, 0.001, 410, 1000), "410");
            return list;
        }

        public static Dictionary<PointPairList, String> specter(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            list.Add(analysis.buildSpecter(model.buildSin(100, 0.001, 11, 1000), 2), "11");
            list.Add(analysis.buildSpecter(model.buildSin(100, 0.001, 110, 1000), 2), "110");
            list.Add(analysis.buildSpecter(model.buildSin(100, 0.001, 250, 1000), 2), "250");
            list.Add(analysis.buildSpecter(model.buildSin(100, 0.001, 410, 1000), 2), "410");
            return list;
        }

        public static Dictionary<PointPairList, String> sum3sin(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            PointPairList list1 = model.buildSin(25, 0.001, 11, 1000);
            PointPairList list2 = model.buildSin(35, 0.001, 41, 1000);
            PointPairList list3 = model.buildSin(30, 0.001, 141, 1000);
            PointPairList listSum = model.addLists(list1, list2);
            listSum = model.addLists(listSum, list3);
            list.Add(list1, "1");
            list.Add(list2, "2");
            list.Add(list3, "3");
            list.Add(listSum, "Amount");
            return list;
        }

        public static Dictionary<PointPairList, String> delAndAnti(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            Processing proc = new Processing();
            PointPairList list1 = model.buildRandom(-100, 100, 1000);
            PointPairList list2 = model.shift(list1, 0, 1000, 1000);
            PointPairList list3 = proc.deleteShift(list1);
            PointPairList list4 = model.addSpikes(list1);
            PointPairList list5 = proc.antiSpike(list4, -100, 100);
            PointPairList list6 = model.addLists(model.trend(-0.5, 1000, true, 1000), model.buildRandom(-100, 100, 1000));
            PointPairList list7 = proc.antiTrend(list1, 10);

            list.Add(list2, "With shift");
            list.Add(list3, "Without shift");
            list.Add(list4, "With spikes");
            list.Add(list5, "Without spikes");
            list.Add(list6, "Trend+Random");
            list.Add(list7, "without trend");
            return list;
        }

        public static Dictionary<PointPairList, String> shiftSin(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            Processing proc = new Processing();
            PointPairList list1 = model.buildSin(100, 0.001, 110, 1000);
            PointPairList list2 = model.shift(list1, 0, 1000, list1.Max(p => p.Y) * 100);
            PointPairList list3 = analysis.buildSpecter(list2, 2);
            PointPairList list4 = proc.deleteShift(list1);
            PointPairList list5 = analysis.buildSpecter(list4, 2);

            list.Add(list2, "Sin 110 With shift");
            list.Add(list3, "Specter");
            list.Add(list4, "Without shift");
            list.Add(list5, "Specter");

            return list;
        }

        public static Dictionary<PointPairList, String> trendSin(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            Processing proc = new Processing();
            PointPairList list1 = model.buildSin(100, 0.001, 110, 1000);
            PointPairList list2 = model.addLists(model.trend(0.5, 1000, true, 1000), list1); ;
            PointPairList list3 = analysis.buildSpecter(list2, 2);
            PointPairList list4 = proc.antiTrend(list1, 100);
            PointPairList list5 = analysis.buildSpecter(list4, 2);

            list.Add(list2, "y(t)+trend");
            list.Add(list3, "Specter");
            list.Add(list4, "Without trend");
            list.Add(list5, "Specter");

            return list;
        }

        public static Dictionary<PointPairList, String> randAndSpikeSpecter(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            Processing proc = new Processing();
            PointPairList list1 = model.buildRandom(-100, 100, 1000);
            PointPairList list2 = analysis.buildSpecter(list1, 2);
            PointPairList list3 = new PointPairList();
            for (int i = 0; i < 1000; i++) list3.Add(i, 0);
            list3 = model.addSpikes(list3, 100, -100);
            PointPairList list4 = analysis.buildSpecter(list3, 2);

            list.Add(list1, "random");
            list.Add(list2, "Specter");
            list.Add(list3, "spikes");
            list.Add(list4, "Specter");

            return list;
        }

        public static Dictionary<PointPairList, String> SinAndRandSpecter(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            Processing proc = new Processing();
            PointPairList list1 = model.buildRandom(-100, 100, 1000);
            list1 = model.addLists(model.buildSin(100, 0.001, 10, 1000), list1);
            PointPairList list2 = analysis.buildSpecter(list1, 2);
            PointPairList list3 = model.addSpikes(model.buildSin(100, 0.001, 10, 1000));
            PointPairList list4 = analysis.buildSpecter(list3, 2);

            list.Add(list1, "random+sin");
            list.Add(list2, "Specter");
            list.Add(list3, "sin+spikes");
            list.Add(list4, "Specter");

            return list;
        }

        public static Dictionary<PointPairList, String> procFile(PointPairList list1)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();
            Processing proc = new Processing();
            Model model = new Model();
            PointPairList list2 = analysis.buildSpecter(list1, 2);
            PointPairList list3 = analysis.buildCorrelation(list1);
            PointPairList list4 = model.buildSin(60, 0.001, 5, 1000);
            PointPairList list5 = analysis.buildCorrelation(list1, list4);
            PointPairList list6 = analysis.buildHistogram(list4, 100);

            list.Add(list1, "signal");
            list.Add(list2, "Specter");
            list.Add(list3, "correlation");
            list.Add(list5, "cross correlation");
            list.Add(list6, "histogram");

            return list;
        }

        public static Dictionary<PointPairList, String> inverseFurie(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();

            PointPairList list1 = model.buildSin(60, 0.001, 5, 1000);
            //PointPairList list2 = analysis.buildSpecter(list1);
            PointPairList list3 = analysis.buildSpecter(list1, 2);
            //PointPairList list4 = analysis.getComplex(list2);
            list.Add(list1, "Sin");
            list.Add(list3, "Specter");
            //list.Add(list4, "Inverse");

            return list;
        }
        public static Dictionary<PointPairList, String> specZero(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();

            PointPairList list1 = model.buildSin(60, 0.001, 100, 1000);
            PointPairList list2 = new PointPairList();
            for (int i = 0; i < 1000; i++)
            {
                if (i < 950) list2.Add(i, list1.ElementAt(i).Y);
                else list2.Add(i, 0);
            }
            PointPairList list3 = analysis.buildSpecter(list2, 2);

            list.Add(list2, "Sin");
            list.Add(list3, "Specter");

            return list;
        }

        public static Dictionary<PointPairList, String> cardio(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();

            PointPairList list1 = model.buildSinCardio(2, 0.005, 10, 200, 0.2);
            PointPairList list2 = new PointPairList();
            for (int i = 0; i < 1000; i++)
            {
                list2.Add(i, 0);
                if (i % 250 == 0) list2.Add(i, 120);
            }
            PointPairList list3 = model.convolution(list2, list1);
            PointPairList list4 = new PointPairList();
            for (int i = 0; i < 1000; i++) list4.Add(i, 0);
            list4 = model.addSpikes(list4, 1, -1);
            PointPairList list5 = model.convolution(list4, list1);

            list.Add(list1, "h");
            list.Add(list3, "y");
            list.Add(list5, "y2");

            return list;
        }
        public static Dictionary<PointPairList, String> cardio2(Model model)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();
            Analysis analysis = new Analysis();

            PointPairList list1 = model.buildSinCardio(2, 0.005, 10, 200, 0.2);
            PointPairList list2 = new PointPairList();
            for (int i = 0; i < 1000; i++)
            {
                list2.Add(i, 0);
                if (i % 250 == 0) list2.Add(i, 120);
            }
            PointPairList list3 = model.convolution(list2, list1);
            PointPairList list4 = new PointPairList();
            for (int i = 0; i < 1000; i++) list4.Add(i, 0);
            list4 = model.addSpikes(list4, 1, -1);
            PointPairList list5 = model.convolution(list4, list1);

            list.Add(list1, "h");
            list.Add(analysis.buildSpecter(list1, 2), "h");
            list.Add(list2, "x");
            list.Add(analysis.buildSpecter(list2, 2), "x");
            list.Add(list3, "y");
            list.Add(analysis.buildSpecter(list3, 2), "y");

            return list;
        }
        public static Dictionary<PointPairList, String> filter(Model model, Analysis analysis, Processing proc, PointPairList listFile)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();

            PointPairList list1 = proc.lpf(32, 0.001, 20);
            PointPairList list2 = list1.Clone();
            list2 = analysis.buildSpecter(list2, 2);
            list2.ForEach(pp => { pp.Y *= list1.Count(); /*pp.X *= 10; */});

            PointPairList list3 = model.convolution(listFile, list1);

            list.Add(list1, "lpw");
            list.Add(list2, "|lpw|");
            //list.Add(listFile, "file");
            //list.Add(analysis.buildSpecter(listFile, 2), "file");
            list.Add(list3, "lpw*file");
            list.Add(analysis.buildSpecter(list3, 2), "|lpw*file|");

            return list;
        }

        public static Dictionary<PointPairList, String> filterS(Model model, Analysis analysis, Processing proc, PointPairList listFile)
        {
            Dictionary<PointPairList, String> list = new Dictionary<PointPairList, String>();

            PointPairList list1 = proc.lpf(32, 0.001, 20);
            list1 = model.convolution(listFile, list1);
            PointPairList list2 = proc.hpf(32, 0.001, 100);
            list2 = model.convolution(listFile, list2);
            PointPairList list3 = proc.bpf(32, 0.001, 20, 100);
            list3 = model.convolution(listFile, list3);
            PointPairList list4 = proc.bsf(32, 0.001, 20, 100);
            list4 = model.convolution(listFile, list4);

            list.Add(list1, "LPF * FILE");
            list.Add(analysis.buildSpecter(list1, 2), "|LPF * FILE|");
            list.Add(list2, "HPF * FILE");
            list.Add(analysis.buildSpecter(list2, 2), "|HPF * FILE|");
            //list.Add(analysis.buildSpecter(listFile, 2), "file");
            list.Add(list3, "BPF * FILE");
            list.Add(analysis.buildSpecter(list3, 2), "|BPF * FILE|");
            list.Add(list4, "BSF * FILE");
            list.Add(analysis.buildSpecter(list4, 2), "|BSF * FILE|");

            return list;
        }


    }
}