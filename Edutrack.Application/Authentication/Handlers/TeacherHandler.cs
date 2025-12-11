using EduTrack.Application.Authentication.Commands;
using EduTrack.Application.Authentication.Dtos;
using EduTrack.Application.Authentication.Queries;
using EduTrack.Domain.Entities;
using EduTrack.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Globalization;

namespace EduTrack.Application.Authentication.Handlers

{
    public class TeacherHandler : IRequestHandler<ListTeachersQuery, PagedResult<TeacherResponseDto>>
    {
        private readonly EduTrackDbContext _context;
        public TeacherHandler(EduTrackDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<TeacherResponseDto>> Handle(ListTeachersQuery request, CancellationToken ct)
        {
            var query = _context.Teachers
            .Include(t => t.Employee)
            .AsQueryable();

            // Filtrar por estado
            if (request.Status.HasValue)
                query = query.Where(t => t.Status == request.Status.Value);

            // Filtrar por departamento (si departmentId != 0)
            if (request.DepartmentId.HasValue && request.DepartmentId.Value != 0)
                query = query.Where(t => t.Employee.DepartmentId == request.DepartmentId.Value);

            //filtrar por nombre
            if (!string.IsNullOrWhiteSpace(request.EmployeeFullName))
                query = query.Where(t => t.Employee.FullName.Contains(request.EmployeeFullName));

            // Total antes de paginar
            var totalCount = await query.CountAsync(ct);

            // Aplicar paginación dinámica
            var items = await query
            .Include(t => t.Employee)
                .ThenInclude(e => e.Department)
            .Include(t => t.Employee)
                .ThenInclude(e => e.Position)
            .OrderBy(t => t.Employee.FullName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new TeacherResponseDto
            {
                TeacherId = t.TeacherId,
                EmployeeId = t.EmployeeId,
                EmployeeFullName = t.Employee.FullName,
                Specialty = t.Specialty,
                Degree = t.Degree,
                HireDate = t.HireDate,
                Status = t.Status,
                CreatedAt = t.CreatedAt,

                DepartmentId = t.Employee.DepartmentId,
                DepartmentName = t.Employee.Department != null ? t.Employee.Department.DepartmentName : string.Empty,

                PositionId = t.Employee.PositionId,
                PositionName = t.Employee.Position != null ? t.Employee.Position.PositionName : string.Empty
            })
            .ToListAsync(ct);

            return new PagedResult<TeacherResponseDto>(items, totalCount, request.Page, request.PageSize);
        }
    }
    public class CreateTeacherHandler : IRequestHandler<CreateTeacherCommand, TeacherResponseDto>
    {
        private readonly EduTrackDbContext _context;
        public CreateTeacherHandler(EduTrackDbContext context) => _context = context;

        public async Task<TeacherResponseDto> Handle(CreateTeacherCommand request, CancellationToken ct)
        {
            var employee = await _context.Employees
                //.Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(e => e.EmployeeId == request.Dto.EmployeeId, ct);

            if (employee == null)
                throw new Exception("Empleado no encontrado.");

            if (await _context.Teachers.AnyAsync(t => t.EmployeeId == request.Dto.EmployeeId, ct))
                throw new Exception("El empleado ya está registrado como profesor.");

            var teacher = new Teacher
            {
                EmployeeId = request.Dto.EmployeeId,
                Specialty = request.Dto.Specialty,
                Degree = request.Dto.Degree,
                HireDate = request.Dto.HireDate,
                Status = request.Dto.Status,
                CreatedBy = request.CurrentUserName
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync(ct);

            return new TeacherResponseDto
            {
                TeacherId = teacher.TeacherId,
                EmployeeId = teacher.EmployeeId,
                EmployeeFullName = employee.FullName,
                Specialty = teacher.Specialty,
                Degree = teacher.Degree,
                HireDate = teacher.HireDate,
                Status = teacher.Status,
                CreatedAt = teacher.CreatedAt,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department != null ? employee.Department.DepartmentName : string.Empty,
                PositionId = employee.PositionId,
                PositionName = employee.Position != null ? employee.Position.PositionName : string.Empty
            };
        }

    }
    public class UpdateTeacherHandler : IRequestHandler<UpdateTeacherCommand, TeacherResponseDto>
    {
        private readonly EduTrackDbContext _context;
        public UpdateTeacherHandler(EduTrackDbContext context) => _context = context;

        public async Task<TeacherResponseDto> Handle(UpdateTeacherCommand request, CancellationToken ct)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Employee).ThenInclude(e => e.Department)
                .Include(t => t.Employee).ThenInclude(e => e.Position)
                .FirstOrDefaultAsync(t => t.TeacherId == request.Dto.TeacherId, ct);

            if (teacher == null)
                throw new Exception("Profesor no encontrado.");

            teacher.Specialty = request.Dto.Specialty;
            teacher.Degree = request.Dto.Degree;
            teacher.HireDate = request.Dto.HireDate;
            teacher.Status = request.Dto.Status;
            teacher.UpdatedAt = DateTime.UtcNow;
            teacher.UpdatedBy = request.CurrentUserName;

            await _context.SaveChangesAsync(ct);

            return new TeacherResponseDto
            {
                TeacherId = teacher.TeacherId,
                EmployeeId = teacher.EmployeeId,
                EmployeeFullName = teacher.Employee.FullName,
                Specialty = teacher.Specialty,
                Degree = teacher.Degree,
                HireDate = teacher.HireDate,
                Status = teacher.Status,
                CreatedAt = teacher.CreatedAt,
                DepartmentId = teacher.Employee.DepartmentId,
                DepartmentName = teacher.Employee.Department != null ? teacher.Employee.Department.DepartmentName : string.Empty,
                PositionId = teacher.Employee.PositionId,
                PositionName = teacher.Employee.Position != null ? teacher.Employee.Position.PositionName : string.Empty
            };
        }
    }
    public class DeleteTeacherHandler : IRequestHandler<DeleteTeacherCommand, bool>
    {
        private readonly EduTrackDbContext _context;
        public DeleteTeacherHandler(EduTrackDbContext context) => _context = context;

        public async Task<bool> Handle(DeleteTeacherCommand request, CancellationToken ct)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherId == request.TeacherId, ct);
            if (teacher == null) return false;

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
    public class ListTeachersHandler : IRequestHandler<ListTeachersQuery, PagedResult<TeacherResponseDto>>
    {
        private readonly EduTrackDbContext _context;

        public ListTeachersHandler(EduTrackDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TeacherResponseDto>> Handle(ListTeachersQuery request, CancellationToken ct)
        {
            var query = _context.Teachers
                .Include(t => t.Employee).ThenInclude(e => e.Department)
                .Include(t => t.Employee).ThenInclude(e => e.Position)
                .AsQueryable();

            if (request.Status.HasValue)
                query = query.Where(t => t.Status == request.Status.Value);

            if (request.DepartmentId.HasValue)
                query = query.Where(t => t.Employee.DepartmentId == request.DepartmentId.Value);

            var items = await query
                .OrderBy(t => t.Employee.FullName)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => new TeacherResponseDto
                {
                    TeacherId = t.TeacherId,
                    EmployeeId = t.EmployeeId,
                    EmployeeFullName = t.Employee.FullName,
                    Specialty = t.Specialty,
                    Degree = t.Degree,
                    HireDate = t.HireDate,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                    DepartmentId = t.Employee.DepartmentId,
                    DepartmentName = t.Employee.Department != null ? t.Employee.Department.DepartmentName : string.Empty,
                    PositionId = t.Employee.PositionId,
                    PositionName = t.Employee.Position != null ? t.Employee.Position.PositionName : string.Empty
                })
                .ToListAsync(ct);

            return new PagedResult<TeacherResponseDto>(items, request.TotalCount, request.Page, request.PageSize);
        }
    }
    public class ExportTeachersPdfHandler : IRequestHandler<ExportTeachersPdfQuery, byte[]>
    {
        private readonly EduTrackDbContext _context;
        private readonly ILogger<ExportTeachersPdfHandler> _logger;
        public ExportTeachersPdfHandler(EduTrackDbContext context, ILogger<ExportTeachersPdfHandler> logger)
        {
            _context = context;
            _logger = logger;
            QuestPDF.Settings.License = LicenseType.Community;
        }
        public async Task<byte[]> Handle(ExportTeachersPdfQuery request, CancellationToken ct)
        {
            try
            {
                var dto = request.Dto;
                var query = _context.Teachers
                .Include(t => t.Employee).ThenInclude(e => e.Position)
                .Include(t => t.Employee).ThenInclude(e => e.Department)
                .AsQueryable();
                if (dto.Status.HasValue)
                    query = query.Where(t => t.Status == dto.Status.Value);

                if (dto.DepartmentId.HasValue && dto.DepartmentId.Value > 0)
                    query = query.Where(t => t.Employee.DepartmentId == dto.DepartmentId.Value);

                if (!string.IsNullOrWhiteSpace(dto.EmployeeFullName))
                    query = query.Where(t => EF.Functions.Like(t.Employee.FullName, $"%{dto.EmployeeFullName}%"));

                var totalCount = await query.CountAsync(ct);

                var rows = await query
                .OrderBy(t => t.Employee.Department.DepartmentName) // Primero por departamento
                .ThenBy(t => t.Employee.FullName)                    // Luego por nombre
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .Select(t => new ExportTeacherRow
                {
                    EmployeeFullName = t.Employee.FullName,
                    DepartmentName = t.Employee.Department.DepartmentName ?? "Sin Departamento",
                    Specialty = t.Specialty ?? "N/A",
                    Degree = t.Degree ?? "N/A",
                    PositionName = t.Employee.Position != null ? t.Employee.Position.PositionName : "N/A",
                    HireDate = t.HireDate,
                    Status = t.Status
                })
                .ToListAsync(ct);

                var stats = new ReportStatistics
                {
                    TotalTeachers = totalCount,
                    ActiveTeachers = rows.Count(r => r.Status),
                    InactiveTeachers = rows.Count(r => !r.Status),
                    TotalPages = (int)Math.Ceiling(totalCount / (double)dto.PageSize),
                    CurrentPage = dto.Page
                };

                var culture = new CultureInfo("es-PE");
                var generatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm", culture);
                var currentYear = DateTime.Now.Year;

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        // Configuración de página - VERTICAL
                        page.Size(PageSizes.A4);
                        page.Margin(1.5f, QuestPDF.Infrastructure.Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Calibri"));

                        // Header
                        page.Header().Element(c => ComposeHeader(c, generatedAt, currentYear, dto));

                        // Content
                        page.Content().Element(c => ComposeContent(c, rows, stats));

                        // Footer
                        page.Footer().Element(c => ComposeFooter(c, generatedAt));
                    });
                });

                var pdfBytes = document.GeneratePdf();
                _logger.LogInformation(
                "PDF generado exitosamente. Registros: {Count}, Tamaño: {Size} KB",
                rows.Count,
                pdfBytes.Length / 1024);

                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando PDF de docentes");
                throw;
            }
        }
        private void ComposeHeader(IContainer container, string generatedAt, int currentYear, ExportTeachersPdfDto filters)
        {
            container.Column(column =>
            {
                column.Spacing(8);

                // Fila principal: Logo + Título + Info
                column.Item().Row(row =>
                {
                    // Logo simulado (cuadro con iniciales)
                    row.ConstantItem(60).Height(60).Border(2)
                        .BorderColor(Colors.Blue.Darken2)
                        .Background(Colors.Blue.Lighten4)
                        .AlignCenter()
                        .AlignMiddle()
                        .Text("ET")
                        .FontSize(24)
                        .Bold()
                        .FontColor(Colors.Blue.Darken3);

                    row.ConstantItem(15); // Espaciador

                    // Información principal
                    row.RelativeItem().Column(col =>
                    {
                        col.Spacing(2);

                        // Nombre de la institución
                        col.Item().Text("EduTrack")
                            .FontSize(22)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);

                        // Subtítulo
                        col.Item().Text("Sistema de Gestión Educativa")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);

                        // Año educativo
                        col.Item().PaddingTop(3).Text($"Año Educativo {currentYear}")
                            .FontSize(9)
                            .Italic()
                            .FontColor(Colors.Blue.Medium);
                    });

                    // Fecha de generación
                    row.ConstantItem(120).AlignRight().Column(dateCol =>
                    {
                        dateCol.Item().Text("Fecha de Generación")
                            .FontSize(8)
                            .FontColor(Colors.Grey.Medium);

                        dateCol.Item().Text(generatedAt)
                            .FontSize(10)
                            .Bold()
                            .FontColor(Colors.Grey.Darken2);
                    });
                });

                // Línea separadora
                column.Item().PaddingVertical(5).LineHorizontal(2).LineColor(Colors.Blue.Darken2);

                // Título del reporte
                column.Item().Background(Colors.Blue.Darken3)
                    .Padding(8)
                    .AlignCenter()
                    .Text("REPORTE DE DOCENTES")
                    .FontSize(14)
                    .Bold()
                    .FontColor(Colors.White);

                // Filtros aplicados (si los hay)
                if (filters.DepartmentId.HasValue ||
                    filters.Status.HasValue ||
                    !string.IsNullOrWhiteSpace(filters.EmployeeFullName))
                {
                    column.Item().PaddingTop(5)
                        .Background(Colors.Grey.Lighten3)
                        .Padding(6)
                        .Row(filterRow =>
                        {
                            filterRow.AutoItem().Text("Filtros: ")
                                .FontSize(8)
                                .Bold()
                                .FontColor(Colors.Grey.Darken2);

                            filterRow.AutoItem().PaddingLeft(5);

                            if (filters.DepartmentId.HasValue)
                            {
                                filterRow.AutoItem().Border(1)
                                    .BorderColor(Colors.Blue.Lighten2)
                                    .Background(Colors.Blue.Lighten4)
                                    .Padding(3)
                                    .Text($"Depto: {filters.DepartmentId.Value}")
                                    .FontSize(7);

                                filterRow.AutoItem().PaddingLeft(5);
                            }

                            if (!string.IsNullOrWhiteSpace(filters.EmployeeFullName))
                            {
                                filterRow.AutoItem().Border(1)
                                    .BorderColor(Colors.Grey.Medium)
                                    .Background(Colors.Grey.Lighten4)
                                    .Padding(3)
                                    .Text($"Búsqueda: {filters.EmployeeFullName}")
                                    .FontSize(7);

                                filterRow.AutoItem().PaddingLeft(5);
                            }

                            if (filters.Status.HasValue)
                            {
                                var statusColor = filters.Status.Value ? Colors.Green.Lighten2 : Colors.Red.Lighten2;
                                var statusBg = filters.Status.Value ? Colors.Green.Lighten4 : Colors.Red.Lighten4;
                                var statusText = filters.Status.Value ? "Solo Activos" : "Solo Inactivos";

                                filterRow.AutoItem().Border(1)
                                    .BorderColor(statusColor)
                                    .Background(statusBg)
                                    .Padding(3)
                                    .Text(statusText)
                                    .FontSize(7);
                            }
                        });
                }
            });
        }
        private void ComposeContent(IContainer container, List<ExportTeacherRow> rows, ReportStatistics stats)
        {
            container.PaddingVertical(10).Column(column =>
            {
                column.Spacing(12);

                // Sección de Estadísticas
                column.Item().Element(c => ComposeStatistics(c, stats));

                // Tabla de docentes
                column.Item().Element(c => ComposeTeachersTable(c, rows));

                // Mensaje si no hay datos
                if (!rows.Any())
                {
                    column.Item().AlignCenter().PaddingVertical(30)
                        .Column(emptyCol =>
                        {
                            emptyCol.Item().Text("📋")
                                .FontSize(40);

                            emptyCol.Item().PaddingTop(10).Text("No se encontraron docentes")
                                .FontSize(14)
                                .Bold()
                                .FontColor(Colors.Grey.Medium);

                            emptyCol.Item().Text("Intenta ajustar los filtros de búsqueda")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Medium);
                        });
                }
            });
        }
        private void ComposeStatistics(IContainer container, ReportStatistics stats)
        {
            var activeProportion = stats.ActiveTeachers / (double)stats.TotalTeachers;
            var inactiveProportion = 1.0 - activeProportion;

            container.Column(mainCol =>
            {
                mainCol.Item().PaddingBottom(5).Text("Resumen Ejecutivo")
                    .FontSize(12)
                    .Bold()
                    .FontColor(Colors.Blue.Darken3);

                mainCol.Item().Row(row =>
                {
                    row.Spacing(10);

                    // Card: Total de Docentes
                    row.RelativeItem().Element(c => CreateStatCard(
                        c,
                        "👥 Total de Docentes",
                        stats.TotalTeachers.ToString(),
                        Colors.Blue.Medium,
                        Colors.Blue.Lighten4
                    ));

                    // Card: Activos
                    row.RelativeItem().Element(c => CreateStatCard(
                        c,
                        "✓ Docentes Activos",
                        stats.ActiveTeachers.ToString(),
                        Colors.Green.Medium,
                        Colors.Green.Lighten4
                    ));

                    // Card: Inactivos
                    row.RelativeItem().Element(c => CreateStatCard(
                        c,
                        "✕ Docentes Inactivos",
                        stats.InactiveTeachers.ToString(),
                        Colors.Red.Medium,
                        Colors.Red.Lighten4
                    ));
                });

                // Barra de progreso (Activos vs Total)
                if (stats.TotalTeachers > 0)
                {
                    var activePercentage = (stats.ActiveTeachers * 100.0) / stats.TotalTeachers;

                    mainCol.Item().PaddingTop(8).Column(progressCol =>
                    {
                        progressCol.Item().Row(labelRow =>
                        {
                            labelRow.RelativeItem().Text($"Tasa de Actividad: {activePercentage:F1}%")
                                .FontSize(8)
                                .Bold()
                                .FontColor(Colors.Grey.Darken2);

                            labelRow.AutoItem().Text($"{stats.ActiveTeachers} de {stats.TotalTeachers}")
                                .FontSize(8)
                                .FontColor(Colors.Grey.Medium);
                        });

                        progressCol.Item().PaddingTop(3).Height(8)
                            .Background(Colors.Grey.Lighten3)
                            .Row(progressRow =>
                            {
                                if (activeProportion > 0)
                                {
                                    progressRow.RelativeItem((float)activeProportion)
                                        .Background(Colors.Green.Medium);
                                }
                                
                                if (inactiveProportion > 0)
                                {
                                    progressRow.RelativeItem((float)inactiveProportion)
                                        .Background(Colors.Grey.Lighten3);
                                }                                
                            });
                    });
                }
            });
        }
        private void CreateStatCard(IContainer container, string title, string value, string borderColor, string bgColor)
        {
            container.Border(2)
                .BorderColor(borderColor)
                .Background(bgColor)
                .Padding(10)
                .Column(col =>
                {
                    col.Item().Text(title)
                        .FontSize(8)
                        .FontColor(Colors.Grey.Darken2);

                    col.Item().AlignCenter().PaddingTop(5).Text(value)
                        .FontSize(28)
                        .Bold()
                        .FontColor(borderColor);
                });
        }
        private void ComposeTeachersTable(IContainer container, List<ExportTeacherRow> rows)
        {
            container.Table(table =>
            {
                // Definir columnas optimizadas para vertical
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2.5f);  // Empleado
                    columns.RelativeColumn(1.5f);  // Especialidad
                    columns.RelativeColumn(1.2f);  // Grado
                    columns.RelativeColumn(1.5f);  // Posición
                    columns.ConstantColumn(55);    // Contratación
                    columns.ConstantColumn(45);    // Estado
                });

                // Header de la tabla
                table.Header(header =>
                {
                    header.Cell().Element(HeaderStyle).Text("Empleado");
                    header.Cell().Element(HeaderStyle).Text("Especialidad");
                    header.Cell().Element(HeaderStyle).Text("Grado");
                    header.Cell().Element(HeaderStyle).Text("Posición");
                    header.Cell().Element(HeaderStyle).Text("Contrat.");
                    header.Cell().Element(HeaderStyle).Text("Estado");

                    static IContainer HeaderStyle(IContainer c)
                    {
                        return c
                            .Background(Colors.Blue.Darken3)
                            .Padding(6)
                            .AlignMiddle()
                            .DefaultTextStyle(x => x
                                .Bold()
                                .FontColor(Colors.White)
                                .FontSize(8));
                    }
                });

                // Filas de datos con alternancia y agrupación por departamento
                string currentDepartment = null;

                foreach (var (row, index) in rows.Select((r, i) => (r, i)))
                {
                    // Verificar si cambia el departamento (separador visual)
                    if (currentDepartment != row.DepartmentName)
                    {
                        currentDepartment = row.DepartmentName;

                        // Fila de separación por departamento
                        table.Cell().ColumnSpan(6)
                            .Background(Colors.Blue.Lighten3)
                            .Padding(5)
                            .Text($"📁 {currentDepartment}")
                            .FontSize(9)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);
                    }

                    // Alternancia de colores (zebra striping)
                    var isEven = index % 2 == 0;
                    var bgColor = isEven ? Colors.White : Colors.Grey.Lighten4;

                    // Celdas de datos
                    table.Cell().Element(c => BodyStyle(c, bgColor))
                        .Text(row.EmployeeFullName);

                    table.Cell().Element(c => BodyStyle(c, bgColor))
                        .Text(row.Specialty);

                    table.Cell().Element(c => BodyStyle(c, bgColor))
                        .Text(row.Degree);

                    table.Cell().Element(c => BodyStyle(c, bgColor))
                        .Text(row.PositionName);

                    table.Cell().Element(c => BodyStyle(c, bgColor))
                        .Text(row.HireDate.ToString("dd/MM/yyyy"));

                    // Estado con color
                    table.Cell().Element(c => BodyStyle(c, bgColor))
                        .Text(row.Status ? "Activo" : "Inactivo")
                        .FontSize(8)
                        .Bold()
                        .FontColor(row.Status ? Colors.Green.Darken1 : Colors.Red.Darken1);

                    static IContainer BodyStyle(IContainer c, string bg)
                    {
                        return c
                            .Background(bg)
                            .Padding(5)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .AlignMiddle()
                            .DefaultTextStyle(x => x.FontSize(8));
                    }
                }
            });
        }
        private void ComposeFooter(IContainer container, string generatedAt)
        {
            container.Column(column =>
            {
                column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                column.Item().PaddingTop(5).Row(row =>
                {
                    // Información izquierda
                    row.RelativeItem().Text("EduTrack © 2025")
                        .FontSize(8)
                        .FontColor(Colors.Grey.Medium);

                    // Centro: Fecha de generación
                    row.RelativeItem().AlignCenter().Text($"Generado: {generatedAt}")
                        .FontSize(8)
                        .FontColor(Colors.Grey.Medium);

                    // Derecha: Paginación
                    row.RelativeItem().AlignRight().Text(text =>
                    {
                        // 1. Definimos el estilo base para todo este bloque de texto AQUÍ ADENTRO
                        text.DefaultTextStyle(x => x.FontSize(8).FontColor(Colors.Grey.Darken1).Bold());

                        // 2. Ahora agregamos el contenido
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                    //row.RelativeItem().AlignRight().Text(text =>
                    //{
                    //    text.Span("Página ");
                    //    text.CurrentPageNumber();
                    //    text.Span(" de ");
                    //    text.TotalPages();
                    //}).FontSize(8).FontColor(Colors.Grey.Darken1).Bold();
                });
            });
        }
    }
}