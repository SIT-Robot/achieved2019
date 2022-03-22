echo off
cls
cd /d %~dp0
for /r . %%a in (.) do @if exist %%a\obj rd /s /q %%a\obj
for /r . %%a in (.) do @if exist %%a\bin rd /s /q %%a\bin
for /r . %%a in (.) do @if exist %%a\.vs rd /s /q %%a\.vs
echo 已清除所有子目录下的.vs、bin、obj目录了。
echo 可以关闭该窗口去SVN Commit了。
pause