using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using iTextSharp.text;


namespace SITRobotSystem_wpf.BLL.FunctionClass
{
    /// <summary>
    /// PDF生成器
    /// </summary>
    class PDFMaker
    {
        
        //private Image obji1 = null;
        //private String objs1 = null;
        //private Image obji2 = null;
        //private String objs2 = null;
        //private Image obji3 = null;
        //private String objs3 = null;
        //private Image obji4 = null;
        //private String objs4 = null;
        //private Image obji5 = null;
        //private String objs5 = null;

        //public PDFMaker(string objs5, Image obji5, string objs4, Image obji4, string objs3, Image obji3, string objs2, Image obji2, string objs1, Image obji1)
        //{
        //    this.objs5 = objs5;
        //    this.obji5 = obji5;
        //    this.objs4 = objs4;
        //    this.obji4 = obji4;
        //    this.objs3 = objs3;
        //    this.obji3 = obji3;
        //    this.objs2 = objs2;
        //    this.obji2 = obji2;
        //    this.objs1 = objs1;
        //    this.obji1 = obji1;
        //}
        private string resultPath;
        private string resultName;

        

        public PDFMaker(string thePath)
        {
            this.resultPath = thePath;
            resultName = null;
        }
        public PDFMaker(string thePath,string theName)
        {
            this.resultPath = thePath;
            resultName = theName;
        }

        public void Maker()
        {
            string time = DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);
            //String filepath = Environment.CurrentDirectory;
            String filepath = Environment.CurrentDirectory;
            filepath = Path.Combine(filepath, "successfulSurf");

            string Filename = "//report-"+ time +".pdf";
            Dictionary<string, int> dict = new Dictionary<string, int>();
            //dict["fop"] = 1;
            Stream fileStream;

            Console.Out.WriteLine("Eintraege: " + dict.Values.Count);
            fileStream = new System.IO.FileStream(filepath+Filename, System.IO.FileMode.Create,FileAccess.ReadWrite);
            doPDF(fileStream, dict,resultPath);

//            System.Diagnostics.Process p;
//            p = new System.Diagnostics.Process();
//            p.StartInfo.FileName = filepath+Filename;
//            p.StartInfo.RedirectStandardOutput = false;
//            p.StartInfo.UseShellExecute = true;
//            p.Start();
        }

        public void doPDF(Stream stream, Dictionary<string, int> dict,string surfphoto)
        {
            try
            {
                string assemblyName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString());
                string assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

                // step 1: create a document
                iTextSharp.text.Document document = new iTextSharp.text.Document();
                // step 2: we set the ContentType and create an instance of the Writer
                PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, stream);

                writer.PageEvent = new MyPdfPageEventHelper(dict);
                /*
                PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, 
                          new System.IO.FileStream(Filename, System.IO.FileMode.Create));
                          */
                // step 3:  add metadata (before document.Open())
                document.AddTitle("The Report of Manipulation and Object recongnition");
                document.AddKeywords("Home Accident");
                document.AddAuthor(" Well-E");
                document.AddProducer();

                // step 4: open the doc
                document.Open();

                // step 5: we Add content to the document
                //TODO: increase font size
                iTextSharp.text.Font font24 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 24);
                iTextSharp.text.Font font18 = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 18);
                iTextSharp.text.Font fontAnchor = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 10,
                                                 iTextSharp.text.Font.UNDERLINE,
                                                 new iTextSharp.text.BaseColor(0, 0, 255));
                iTextSharp.text.Chunk bullet = new iTextSharp.text.Chunk("\u2022", font18);


                Chunk ck = new Chunk("ShangHai Institute of Technology");

                Paragraph pg = new Paragraph(ck);

                pg.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;

                document.Add(pg);

                document.Add(new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("The Report of Manipulation and Object recongnition", font24)));

                document.Add(new iTextSharp.text.Paragraph("\n"));
                
                iTextSharp.text.List list = new iTextSharp.text.List(false, 20);  // true= ordered, false= unordered

                Paragraph pe = new iTextSharp.text.Paragraph("No");
                pe.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                //新的一页，已注释
                //document.NewPage();
                String ourphoto = Environment.CurrentDirectory;
                ourphoto = Path.Combine(ourphoto, resultPath);
                iTextSharp.text.Image image11 = iTextSharp.text.Image.GetInstance(ourphoto);
                image11.ScalePercent(30f);
                image11.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
                document.Add(image11);

                list.ListSymbol = bullet;       // use "bullet" as list symbol
                list.Add(new iTextSharp.text.ListItem("on " + System.DateTime.Now.ToString("dddd, MMM d, yyyy")));
                list.Add(new iTextSharp.text.ListItem("at " + System.DateTime.Now.ToString("hh:mm:ss tt zzzz")));
                list.Add(new iTextSharp.text.ListItem("物品名字:" + resultName));

                document.Add(list);

                // step 6: Close document

                document.Close();

            }
            catch (iTextSharp.text.DocumentException ex)
            {
                System.Console.Error.WriteLine(ex.StackTrace);
                System.Console.Error.WriteLine(ex.Message);
            }



        }
    }

     public class MyPdfPageEventHelper : PdfPageEventHelper
    {

        Dictionary<string, int> dict = null;
        public MyPdfPageEventHelper(Dictionary<string, int> dict)
        {

            this.dict = dict;
        }
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            //System.Console.Out.WriteLine("OnStartPage " );

        }

        public void OnLocalDestination(PdfWriter writer, Document document, PdfDestination dest, string marker)
        {
            Console.Out.WriteLine("OnLocalDestination text:" + marker + " writer.CurrentPageNumber: " + writer.CurrentPageNumber);
            dict[marker] = writer.CurrentPageNumber;
        }


    }
}
