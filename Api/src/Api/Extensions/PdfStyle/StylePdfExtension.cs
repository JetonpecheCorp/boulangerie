using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Api.Extensions.PdfStyle;

public static class StylePdfExtension
{
    public static IContainer StyleHeaderTableau(this IContainer container)
    {
        return container
            .BorderBottom(2)
            .BorderColor(Colors.Black)
            .Background(Colors.Grey.Lighten3)
            .PaddingVertical(5)
            .PaddingHorizontal(10)
            .ShowOnce();
    }

    public static IContainer Style(this IContainer container)
    {
        return container
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten1)
            .PaddingVertical(5)
            .PaddingHorizontal(10);
    }

    public static IContainer CellStyleHeaderTableau(this TableDescriptor container)
    {
        return container.Cell().Style();
    }

    public static IContainer CellStyle(this TableDescriptor container)
    {
        return container
            .Cell()
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten1)
            .PaddingVertical(5)
            .PaddingHorizontal(10);
    }
}
