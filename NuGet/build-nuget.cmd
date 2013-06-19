mkdir lib
mkdir lib\win8
copy ..\Hiromi.WindowsStore\bin\Debug\Hiromi.WindowsStore.dll lib\win8\
copy ..\Hiromi.WindowsStore\bin\Debug\MonoGame.Framework.dll lib\win8\
nuget pack Hiromi.WindowsStore.nuspec
