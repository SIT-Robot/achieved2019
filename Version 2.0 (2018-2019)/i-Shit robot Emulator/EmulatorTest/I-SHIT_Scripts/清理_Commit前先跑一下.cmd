echo off
cls
cd /d %~dp0
for /r . %%a in (.) do @if exist %%a\obj rd /s /q %%a\obj
for /r . %%a in (.) do @if exist %%a\bin rd /s /q %%a\bin
for /r . %%a in (.) do @if exist %%a\.vs rd /s /q %%a\.vs
del /q/a/f .\Data\KinectShotImages\*.*
echo �����������Ŀ¼�µ�.vs��bin��objĿ¼��kinect��Ƭ��
echo ���Թرոô���ȥSVN Commit��
echo �ټ�
pause