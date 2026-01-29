#!/usr/bin/env powershell
# Script para compilar y ejecutar el Sistema de Ventas
# Uso: .\ejecutar-aplicacion.ps1

$rutaProyecto = "C:\Users\jqalvarado\Documents\CAPACITACION\R\ING SOFT D SAVIO\DESARROLLO I\SistemaVentas"
$rutaCsproj = "$rutaProyecto\SistemaVentas.csproj"

Write-Host "===========================================" -ForegroundColor Cyan
Write-Host "Sistema de Ventas - Ejecución" -ForegroundColor Green
Write-Host "===========================================" -ForegroundColor Cyan
Write-Host ""

# Cambiar al directorio del proyecto
Set-Location $rutaProyecto

# Compilar el proyecto
Write-Host "🔨 Compilando proyecto..." -ForegroundColor Yellow
dotnet build $rutaCsproj -c Debug

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error en compilación. Abortando..." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Compilación exitosa" -ForegroundColor Green
Write-Host ""

# Ejecutar la aplicación
Write-Host "🚀 Iniciando aplicación..." -ForegroundColor Yellow
Write-Host "💡 Recuerda:" -ForegroundColor Cyan
Write-Host "  1. Ir a Reportes > Ventas por Período"
Write-Host "  2. Se abrirá el formulario de reporte de productos"
Write-Host "  3. Haz clic en 'Actualizar' para cargar los datos"
Write-Host "  4. Exporta a CSV si lo deseas"
Write-Host ""

dotnet run --project $rutaCsproj --no-build -c Debug
