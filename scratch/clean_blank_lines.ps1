
$files = Get-ChildItem -Recurse -Include *.cs

foreach ($file in $files) {
    $content = [System.IO.File]::ReadAllText($file.FullName)
    
    # 1. Substituir 3 ou mais quebras de linha por 2 (mantém no máximo uma linha em branco)
    $content = $content -replace '(\r?\n){3,}', "`r`n`r`n"
    
    # 2. Remover linha em branco logo após {
    $content = $content -replace '\{\s*(\r?\n)\s*(\r?\n)', "{`r`n"
    
    # 3. Remover linha em branco logo antes de }
    $content = $content -replace '(\r?\n)\s*(\r?\n)\s*\}', "`r`n}"
    
    # 4. Remover linhas em branco no início do arquivo
    $content = $content.TrimStart()
    
    # 5. Remover linhas em branco no fim do arquivo
    $content = $content.TrimEnd() + "`r`n"
    
    [System.IO.File]::WriteAllText($file.FullName, $content)
}
