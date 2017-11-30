using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using processAI1.Agent;
using System.Windows.Forms;

namespace processAI1
{
    public enum ColorPlayer { White, Black };
    class Program
    {

       
       static void Main(string[] args)
        {
            
            IComunicationProtocol protocol = new UciProtocol();
            protocol.Run();
          

        }
    }
}
