using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace NADIA
{
    class Program
    {
        //public SpeechRecognitionEngine recognitionEngine;
        public SpeechRecognizer recognizer;
        string message;
        string outputMessage = "";
        SpeechSynthesizer speaker = new SpeechSynthesizer();
        public static void Main()
        {
            Program prgrm = new Program();
            prgrm.Init();
            //process commands
            string thisListeningMessage = "";
            while (true)
            {
                string timenow = DateTime.Now.Second.ToString();
                if (timenow != thisListeningMessage)
                {
                    thisListeningMessage = timenow;
                    Console.WriteLine("Listening...");
                }
                //prgrm.message = prgrm.recognize();
                if (!string.IsNullOrEmpty(prgrm.message))
                {
                    if (prgrm.command(prgrm.message))
                        prgrm.message = "";
                }
            }
        }
        public void Init()
        {
            recognizer = new SpeechRecognizer();
            recognizer.Enabled = true;
            recognizer.SpeechRecognized += (s, args) =>
            {
                foreach (RecognizedWordUnit word in args.Result.Words)
                {
                    if (word.Confidence > 0.5f)
                        message += word.Text + " ";
                }
            };
            recognizer.LoadGrammar(new DictationGrammar());

            //recognitionEngine = new SpeechRecognitionEngine();
            //recognitionEngine.SetInputToDefaultAudioDevice();
            //recognitionEngine.SpeechRecognized += (s, args) =>
            //{
            //    foreach (RecognizedWordUnit word in args.Result.Words)
            //    {
            //        if (word.Confidence > 0.5f)
            //            message += word.Text + " ";
            //    }

            //};
            //recognitionEngine.LoadGrammar(new DictationGrammar());
            if (!string.IsNullOrEmpty(message) && outputMessage != message)
            {
                outputMessage = message;
                Console.WriteLine(message);
            }
        }

        public void speak(string message)
        {

            speaker.Rate = 1;
            speaker.Volume = 100;
            speaker.Speak(message);
        }
        //public string recognize()
        //{
        //    if (!string.IsNullOrEmpty(message) && outputMessage != message)
        //    {
        //        outputMessage = message;
        //        Console.WriteLine(message);
        //    }



        //    recognitionEngine.RecognizeAsyncStop();
        //    return message;
        //}
        public bool command(string message)
        {
            bool executedCommand = false;
            switch (message)
            {
                case "hi":
                case "high ":
                    speak("Hello Sir");
                    if (!string.IsNullOrEmpty(message) && outputMessage != message)
                    {
                        outputMessage = message;
                        Console.WriteLine(message);
                    }
                    message = "";
                    executedCommand = true;
                    break;
                case "nadia chrome ":
                    speak("Opening Google Chrome");
                    speak("Please Wait");
                    Console.WriteLine("Please Wait");
                    System.Diagnostics.Process.Start("Google Chrome.lnk");
                    message = "";
                    executedCommand = true;
                    break;
                case "nadia time ":
                    speak("Sir The Time is " + DateTime.Now.ToString("h:mm tt"));
                    message = "";
                    executedCommand = true;
                    break;
                case "nadia date ":
                    speak("Sir The Date is " + DateTime.Now.Date);
                    message = "";
                    executedCommand = true;
                    break;
                case "nadia day ":
                    speak("Sir The Day is " + DateTime.Now.DayOfWeek);
                    message = "";
                    executedCommand = true;
                    break;
                case "thank you ":
                    speak("You're welcome sir");
                    message = "";
                    executedCommand = true;
                    break;
                case "":
                    break;
                default:
                    string[] messageArr = message.Split(' ');
                    string lastThreeWords = "";
                    string lastTwoWords = "";
                    if (messageArr.Length > 3)
                        lastThreeWords = messageArr[messageArr.Length - 4] + messageArr[messageArr.Length - 3] + messageArr[messageArr.Length - 2];
                    if (messageArr.Length > 2)
                        lastTwoWords = messageArr[messageArr.Length - 3] + messageArr[messageArr.Length - 2];
                    if (lastTwoWords == "forgetthat" ||
                        lastTwoWords == "ignorethat" ||
                        lastThreeWords == "forgetthat")
                    {
                        speak("Ignoring that sir.");
                        executedCommand = true;
                    }
                    else if (!string.IsNullOrEmpty(message) && outputMessage != message)
                    {
                        outputMessage = message;
                        Console.WriteLine(message);
                    }
                    break;
            }
            return executedCommand;
        }
    }
}
