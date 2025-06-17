using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Alexis.Dashboard.Helper;


public class ApkReader
{
    private readonly IWebHostEnvironment _env;
    private readonly string[] _aaptArguments = { "dump", "badging", "" };

    public ApkReader(IWebHostEnvironment env)
    {
        _env = env;
    }

    public ApkInfo GetFileInfo(string apkPath)
    {
        string manifest = ReadApk($"\"{apkPath}\"");
        return new ApkInfo(manifest);
    }

    private string ReadApk(string apkPath)
    {
        _aaptArguments[2] = apkPath;
        return ExecuteCommand("aapt.exe", _aaptArguments);
    }

    private string ExecuteCommand(string fileName, string[] args)
    {
        string exePath = Path.Combine(_env.ContentRootPath, "Resources", fileName);
        if (!File.Exists(exePath))
            throw new FileNotFoundException("aapt.exe not found.");

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = string.Join(" ", args),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8
            }
        };

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output;
    }
}

public class ApkInfo
{
    public string AllManifest { get; }
    public string ApkName { get; }
    public string VersionName { get; }
    public string PackageName { get; }
    public string SdkVersion { get; }
    public string TargetSdkVersion { get; }
    public string VersionCode { get; }
    public string NativeCode { get; }

    public ApkInfo(string manifest)
    {
        AllManifest = manifest;
        ApkName = GetAttribution(@"application-label:'(.*?)'");
        VersionName = GetAttribution(@"versionName='(.*?)'");
        PackageName = GetAttribution(@"package: name='(.*?)'");
        SdkVersion = GetAttribution(@"sdkVersion:'(.*?)'");
        TargetSdkVersion = GetAttribution(@"targetSdkVersion:'(.*?)'");
        VersionCode = GetAttribution(@"versionCode='(.*?)'");
        NativeCode = GetAttribution(@"native-code: '(.*?)'");
    }

    public override string ToString()
    {
        return $"apkName: {ApkName} | versionName: {VersionName} | packageName: {PackageName} | " +
               $"sdkVersion: {SdkVersion} | targetSdkVersion: {TargetSdkVersion} | " +
               $"versionCode: {VersionCode} | nativeCode: {NativeCode}";
    }

    private string GetAttribution(string pattern)
    {
        var match = Regex.Match(AllManifest, pattern, RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
}