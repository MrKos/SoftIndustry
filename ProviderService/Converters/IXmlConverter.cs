using TestTask.Common.DAL.Models;

namespace ProviderService.Converters
{
    public interface IConverter
    {
        Measurement Convert(byte[] content);
    }
}