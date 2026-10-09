Stop-Service -Name MySQL80 -Force
$initFile = "C:\Users\91735\Documents\mysql-init.txt"
"ALTER USER 'root'@'localhost' IDENTIFIED BY 'Admin@123';" | Out-File -FilePath $initFile -Encoding ASCII
$mysqld = "C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqld.exe"
$defaults = "C:\ProgramData\MySQL\MySQL Server 8.0\my.ini"

Write-Host "Resetting MySQL password... Please wait 15 seconds."
$process = Start-Process -FilePath $mysqld -ArgumentList "--defaults-file=`"$defaults`"", "--init-file=`"$initFile`"" -PassThru -WindowStyle Hidden

Start-Sleep -Seconds 15

Stop-Process -Id $process.Id -Force
Start-Sleep -Seconds 2

Start-Service -Name MySQL80
Write-Host "Password reset complete! You can close this window."
Start-Sleep -Seconds 5
