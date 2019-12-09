using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QuanLyLinhKien
{
    class ReadFileConn
    {
        public String LoadConn()
        {
            string s;
            //String filePath = "D:\\DH\\đồ án lập trình window\\QuanlyLinhKien\\QuanLyLinhKien\\QuanLyLinhKien\\data.txt";
            String filePath = "data.txt";
            FileStream fs = new FileStream(filePath, FileMode.Open);
            StreamReader sReader = new StreamReader(fs, Encoding.UTF8);
            String giatri1 = sReader.ReadLine();
            String giatri2 = sReader.ReadLine();
            String giatri3 = sReader.ReadLine();
            String giatri4 = sReader.ReadLine();
            if (giatri3.Trim().Length == 0 && giatri4.Trim().Length == 0)
                s = @"Data source = " + giatri1 + "; initial catalog = " + giatri2 + "; integrated security = true";
            else
                s = @"Data source = " + giatri1 + "; initial catalog = "+giatri2+"; User id = "+giatri3+";password = "+ giatri4;
            fs.Close();
            sReader.Close();
            return s;
        }
    }
}
