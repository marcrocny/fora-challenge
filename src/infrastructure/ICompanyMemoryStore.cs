using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fora.Challenge.Entity;

namespace Fora.Challenge.Impl;

public interface ICompanyMemoryStore
{
    void Load(IEnumerable<Company> companies);
}
