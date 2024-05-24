# Mapper Guide
We use [Mapster](https://github.com/MapsterMapper/Mapster) tool to generate mapper classes to map between domain entities and DTOs. 
This guide provides instructions on creating and updating mapper classes.

## Creating DTOs
- Navigate to the `ProjectName.Shared.Models` project.
- Create a new record type for the DTO.
- Follow the naming convention: `{EntityName}Dto`, where `EntityName` is the name of the entity (e.g., `InvoiceDto`).

### DTO Code Sample
```csharp
public record InvoiceDto
{
    public int Id { get; init; }
    public string InvoiceNumber { get; init; }
    public decimal Amount { get; init; }
    public DateTime DueDate { get; init; }
}
```

## Creating Mappers
1. **Create a Mapper interface**:
    - Navigate to the `ProjectName.Mappings` project.
    - Create a new interface for the mapper.
    - Follow the naming convention: `I{EntityName}Mapper`, where `EntityName` is the name of the entity (e.g., `IInvoiceMapper`).
    - Annotate your interface with `[Mapper]` in order for tool to pickup for generation.
    - Add mapping method signatures like these: `EntityDto ToDto(Entity)` and `Entity ToEntity(EntityDto)`.
    
2. **Generate Mappers**:
    - Run the [generate-mappers.cmd](../src/Alikai.Factoring.Mappings/Scripts/generate-mappers.cmd) script to generate mapper classes.
    - The script will generate mapper classes for all interfaces annotated with [Mapper] in the `ProjectName.Mappings` project, in the `Mappers` folder.
   
> **Note**: To clean up generated files, you can run the [cleanup-mappers.cmd](../src/content/ProjectTemplates/DomainDrivenDesign/Server/Company.Project.Mappings/Scripts/cleanup-mappers.cmd) script.

3. **Add Extension Methods**:
    - Create a new static class with the name in the `{EntityName}Ext` in `Extensions` folder.
    - Add private readonly static field with the name `Mapper` and create an instance.
    - Add extension methods to map between entity and DTO.
    - Example:
    ```csharp
    public static class InvoiceExt
    {
        private static readonly IInvoiceMapper Mapper = new InvoiceMapper();

        public static InvoiceDto ToDto(this Invoice invoice)
        {
            return Mapper.ToDto(invoice);
        }

        public static Invoice ToEntity(this InvoiceDto invoiceDto)
        {
            return Mapper.ToEntity(invoiceDto);
        }
    }
    ```

### Mapper Code Sample
```csharp
[Mapper]
public interface IInvoiceMapper
{
    InvoiceDto ToDto(Invoice invoice);
    Invoice ToEntity(InvoiceDto invoiceDto);
}
```

## Usage Example
- Use extension methods to map between entity and DTO.
```csharp
public class InvoiceService
{
    public InvoiceDto GetInvoice(int id)
    {
        var invoice = _invoiceRepository.Get(id);
        return invoice.ToDto();
    }
}
```
