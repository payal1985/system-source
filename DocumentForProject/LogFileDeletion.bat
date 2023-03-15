forfiles /p "C:\temp\Logs" /s /m *.log /D -30 /C "cmd /c del @path"
