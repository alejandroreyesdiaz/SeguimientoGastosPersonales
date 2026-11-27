using System;
using System.Collections.Generic;
using System.Text;

namespace SeguimientoGastosPersonales.Infrastructure.Queries
{
    public static class ReportesQueries
    {
        public static string ResumenPorCategoriaMySql = @"
            SELECT 
                c.nombre AS Categoria,
                m.tipo AS Tipo,
                COUNT(m.id) AS CantidadMovimientos,
                SUM(m.importe) AS TotalMonto
            FROM movimientos m
            INNER JOIN categorias c ON m.categoria_id = c.id
            WHERE m.usuario_id = @UsuarioId
            GROUP BY c.nombre, m.tipo
            ORDER BY TotalMonto DESC;";
    }
}