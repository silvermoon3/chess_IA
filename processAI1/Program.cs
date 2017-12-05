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
using processAI1.Board;

namespace processAI1
{
    
    public enum ColorPlayer { White, Black };
    class Program
    {
        
        static void Main(string[] args)
        {
            Bit.Init();
            Material.init();
            Hash.Init();
            Castling.init();
            Attack.init();
            PieceSquareTable.init();
            
           IComunicationProtocol protocol = new UciProtocol();
            protocol.Run();

        }
    }
}
