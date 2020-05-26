@echo off
xcopy .\deploy-version.txt ..\ /q /y
xcopy .\deploy-version.txt .\web-build\ /q /y