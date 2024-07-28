@echo off
copy bin\Debug\net8.0 C:\Apps\DirToM3u
md C:\Apps\DirToM3u\src
copy DirToM3u.csproj C:\Apps\DirToM3u\src
copy Program.cs C:\Apps\DirToM3u\src