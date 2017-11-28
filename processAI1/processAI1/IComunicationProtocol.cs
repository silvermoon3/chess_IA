using System;
using System.Windows.Forms.VisualStyles;

namespace processAI1
{
    public interface IComunicationProtocol
    {
        /**
         * Start protocol loop util it's quit.
         * 
         * */
        void Run();
    }
}