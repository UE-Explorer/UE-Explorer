for %%f in (".\*.*") do (
    C:\glslang-master-windows-x64-Release\bin\glslangValidator.exe -V -o ".\%%~nxf.spv" ".\%%~nxf"
)