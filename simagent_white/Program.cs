using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using System.IO;
using System.Threading;
using TestStack.White.Factory;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.WindowsAPI;

namespace simagent_white
{
    class Program
    {
        static void Main(string[] args)

        {
            Application simAgent = Application.Launch("c:\\simagentPkg64\\simagent64.exe");

            try {
       
            bool script1 = SimAgentTasks.RunScript("C:\\simagentPkg64\\Package\\CreateAgentOnce.scp", 101, simAgent);
            bool script2 = SimAgentTasks.RunScript("C:\\simagentPkg64\\Package\\PWSendEndpointTpWcPropsBase", 17, simAgent);
            bool script3 = SimAgentTasks.RunScript("C:\\simagentPkg64\\Package\\SendEpoEventwithDiffrent.scp", 17, simAgent);

                if (!(script1 && script2 && script3))
                {

                    throw new Exception("An error has occurred when executing the SimAgent scripts");

                }

                else {

                    Console.WriteLine("All scripts executed successfully"); 
                }

            }

            catch (Exception ex) {


                    Console.WriteLine(ex.ToString());
                
            }

            simAgent.Close();

        }
                    

    }

    class SimAgentTasks
    {

        public static bool RunScript(string scriptPath, int agentCount, Application simAgent)
        {

            try
            {

                Window mainWindow = simAgent.GetWindow("McAfee ePO 5.10.0 Agent Simulator", InitializeOption.NoCache);

                Button browseScripts = (Button)mainWindow.Get(SearchCriteria.ByAutomationId("1007"));
                browseScripts.Click(); 
                Thread.Sleep(2000);

                var openFileWindow = mainWindow.ModalWindow("Open Script File", InitializeOption.NoCache);
                mainWindow.WaitWhileBusy();
                var filenameTextBox = openFileWindow.Get<TextBox>(SearchCriteria.ByAutomationId("1148"));
                filenameTextBox.SetValue(scriptPath);
                openFileWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                Thread.Sleep(2000);
                TextBox numAgents = (TextBox)mainWindow.Get(SearchCriteria.ByAutomationId("1004"));
                numAgents.Text = agentCount.ToString();
                Button startButton = (Button)mainWindow.Get(SearchCriteria.ByAutomationId("1005"));
                startButton.Click();
                Thread.Sleep(1000);

                int timeoutCount = 0;
                while (!(startButton.Enabled) && timeoutCount <= 60)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Script running..." + scriptPath);
                    timeoutCount++;
                }

                return true; 
            }

            catch (Exception ex) {

                Console.WriteLine("Error: " + ex.ToString());
                return false;
                

            }



        }




    }


}
