param(
    [parameter(mandatory=$true)]
    [string]
    $repositoryRoot,
    
    [parameter()]
    [string]
    $installRoot = $env:ProgramFiles
)

function Get-MsBuildPath {
    $registryLocation = 'HKLM:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0\'    
    $registryEntry = Get-ItemProperty $registryLocation -Name 'MSBuildToolsPath'
    
    return $registryEntry.MSBuildToolsPath
}

function Build-Project {
    $MSBuild = Join-Path (Get-MsBuildPath) 'msbuild.exe'
    $solutionFile = (Join-Path $repositoryRoot 'BuildMonitor.sln')
    $buildArgs = "/p:Configuration=Release"

    & $MSBuild $solutionFile `
                $buildArgs
}

function Test-Service {
    $service = Get-Service TfsBuildMonitor -ErrorAction 'SilentlyContinue'
    return $service -ne $null
}

function Install-Service {
    $serviceExists = Test-Service

    if ($serviceExists) {
        Stop-Service $applicationName
    } 

    $outputPath = Join-Path $repositoryRoot 'BuildMonitor.Service\bin\Release'
    $installPath = Join-Path $installRoot $applicationName

    if (-not (Test-Path $installPath)) {
        New-Item $installPath -Type Directory
    }

    Get-ChildItem $outputPath | foreach { Copy-Item $_.FullName $installPath }

    if (-not $serviceExists) {
        $executablePath = Join-Path $installPath 'BuildMonitor.Service.exe'
        New-Service `
            -Name $applicationName `
            -BinaryPathName $executablePath `
            -StartupType Automatic `
            -Credential (Get-Credential) `
            -DisplayName $applicationName `
            -Description 'A build monitor service to indicate success/failure of a build via a Delcomm light'        
    }   
}

$applicationName = 'TfsBuildMonitor'

Build-Project
Install-Service
Start-Service $applicationName

#what about the delcom stuff?