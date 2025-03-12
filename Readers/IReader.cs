namespace Invoicer.Readers
{
    interface IReader
    {
         Task ReadFileAsync(Stream ExcelFile);
    }
}
