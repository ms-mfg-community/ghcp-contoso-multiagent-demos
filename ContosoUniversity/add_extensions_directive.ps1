$controllersPath = "C:\Users\codycarlson\git\dotnet-migration-copilot-samples\ContosoUniversity\ContosoUniversity.Web\Controllers"
$controllerFiles = Get-ChildItem -Path $controllersPath -Filter "*.cs"

foreach ($file in $controllerFiles) {
    $content = Get-Content -Path $file.FullName -Raw
    
    # Check if the using directive already exists
    if ($content -notmatch "using ContosoUniversity\.Web\.Extensions;") {
        # Find the last using statement
        $lastUsingIndex = $content.LastIndexOf("using ", [System.StringComparison]::OrdinalIgnoreCase)
        $lastUsingSemicolonIndex = $content.IndexOf(";", $lastUsingIndex)
        
        if ($lastUsingIndex -ge 0 -and $lastUsingSemicolonIndex -ge 0) {
            # Insert the new using directive after the last using statement
            $newContent = $content.Substring(0, $lastUsingSemicolonIndex + 1) + "`nusing ContosoUniversity.Web.Extensions;" + $content.Substring($lastUsingSemicolonIndex + 1)
            
            # Write the modified content back to the file
            Set-Content -Path $file.FullName -Value $newContent
            
            Write-Host "Added extension using directive to $($file.Name)"
        }
    } else {
        Write-Host "File $($file.Name) already has the extension using directive"
    }
}
