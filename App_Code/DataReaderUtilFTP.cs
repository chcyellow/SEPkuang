using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Net;
using System.IO;

/// <summary>
///DataReaderUtilFTP 的摘要说明
/// </summary>
public class DataReaderUtilFTP
{
    public DataReaderUtilFTP()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 读取FTP站点下的XML文件，返回FTP站点上的根目录下的所有xml文件名称
    /// </summary>
    /// <param name="ftp">FTP站点</param>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public static String readerFtp(String ftp, String username, String password)
    {
        StringBuilder result = new StringBuilder();
        FtpWebRequest reqFTP;
        try
        {
            String ftpserver;
            if (ftp.StartsWith("ftp"))
            {
                if (ftp.StartsWith("ftp://"))
                {
                    ftpserver = ftp;
                }
                else
                {
                    ftpserver = ftp + "//:";
                }
            }
            else
            {
                if (ftp.EndsWith("/"))
                {
                    ftpserver = "ftp://" + ftp;
                }
                else
                {
                    ftpserver = "ftp://" + ftp + "/";
                }
            }
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpserver));
            reqFTP.UsePassive = false;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(username, password);
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
            string line = reader.ReadLine();
            while (line != null)
            {
                if (line.EndsWith(".xml"))
                {
                    result.Append(line);
                    result.Append("\n");
                }
                line = reader.ReadLine();
            }
            result.Remove(result.ToString().LastIndexOf('\n'), 1);
            reader.Close();
            response.Close();
            Console.Write(result.ToString());
            return result.ToString();
        }
        catch (Exception ex)
        {
            return result.ToString();
        }
    }

    /// <summary>
    /// 读取FTP站点下指定文件名的内容
    /// </summary>
    /// <param name="ftp"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static String readerFtpFile(String ftp, String username, String password, String filename,string MainDeptNumber)
    {        
        StringBuilder result = new StringBuilder();
        FtpWebRequest reqFTP;
        try
        {
            String ftpserver = ftp + filename;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpserver));
            reqFTP.UsePassive = false;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(username, password);
            //reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
            string line = reader.ReadLine();
            while (line != null)
            {
                if (MainDeptNumber != "")
                {
                    if (line.Trim().StartsWith(MainDeptNumber))
                        result.Append(line + "\n");
                }
                else
                    result.Append(line + "\n");
                line = reader.ReadLine();
            }
            result.Remove(result.ToString().LastIndexOf('\n'), 1);
            reader.Close();
            response.Close();
            Console.Write(result.ToString());
            return result.ToString();
        }
        catch (Exception ex)
        {
            Console.Write(result.ToString());
            return result.ToString();
        }
    }

    /// <summary>
    /// 读取本地文件系统目录的文件，返回文件内容
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static String readerLocalFile(String path, string MainDeptNumber)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader m_streamReader = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312"));
        m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
        string arry = "";
        string strLine = m_streamReader.ReadLine();
        while (strLine != null)
        {
            if (MainDeptNumber != "")
            {
                if (strLine.Trim().StartsWith(MainDeptNumber))
                    arry += strLine + "\n";
            }
            else
                arry += strLine + "\n";
            strLine = m_streamReader.ReadLine();
        }
        arry.Remove(arry.LastIndexOf('\n'), 1);
        m_streamReader.Close();
        m_streamReader.Dispose();
        fs.Close();
        fs.Dispose();
        Console.Write(arry);
        Console.ReadLine();

        return arry;
    }

}
