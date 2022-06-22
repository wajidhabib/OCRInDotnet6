using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using Tesseract;

namespace OCRExample.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Text { get; set; }


        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            List<Rectangle> rectangles = new List<Rectangle>();
           //location to testdata for eng.traineddata
           var path = Path.Combine( Directory.GetCurrentDirectory() ,"tessdata");
            //your sample image location
            var sourceFilePath = Path.Combine(Directory.GetCurrentDirectory() , "Images","sample.png");
            using (var engine = new TesseractEngine(path, "eng"))
            {
                engine.SetVariable("user_defined_dpi", "70"); //set dpi for supressing warning
               
                
                using (var img = Pix.LoadFromFile(sourceFilePath))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();
                        Console.WriteLine();
                        Console.WriteLine("---Image Text---");
                        Console.WriteLine();
                        Console.WriteLine(text);

                        Text += text;

                    }


                }

                //--------------------
                
                using (var img = Pix.LoadFromFile(sourceFilePath))
                {
                    PageIteratorLevel level= new PageIteratorLevel();
                

                    using (var page = engine.Process(img))
                    {
                        level = PageIteratorLevel.Word;
                         
                        List<Rectangle> boxes = page.GetSegmentedRegions(level);
                         
                        rectangles.AddRange(boxes);
                    }


                }
 Text += "---------------------  <br />"; 
                foreach (var item in rectangles)
                {

                    Text += " <br />";

                    Text += $"    left-top :{item.Left}, {item.Top}, right-top : {item.Right},{item.Top}, right-bottom: {item.Right},{item.Bottom} , left-bottom: {item.Left},{item.Bottom}";

                   

                }
 Text += "---------------------  <br />";
            }

            

        }
    }
}