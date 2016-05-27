using System.Collections.Generic;

namespace Resolver
{
    public interface IDnsSecOperations
    {
        IList<DSRecord> CalculateRecords(IList<DsInputData> list);
        DSRecord CalculateRecord(DsInputData data);
    }
}
