@echo off
set DEPLOY_PATH=C:\Apps\DirToM3u
copy bin\Debug\net8.0 "%DEPLOY_PATH%"
md "%DEPLOY_PATH%\src"
copy *.csproj "%DEPLOY_PATH%\src"
copy *.cs "%DEPLOY_PATH%\src"