using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;


namespace RobotTracktorBrain
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine("start loading brain...");
            Brain.Instance.brainMap = BrainDeserializer.DeserializeBrainMap($"E:/workspace/robot_tracktor/brain_repo", Brain.BRAIN_WIDTH, Brain.BRAIN_HEIGHT, Brain.BRAIN_DEPTH);
            Console.WriteLine("brain load finished");
            Application.Run(new MovingSquareForm(Brain.Instance.brainMap));
        }
    }
}
