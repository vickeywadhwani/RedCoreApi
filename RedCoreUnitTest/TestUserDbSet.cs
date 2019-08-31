using System;
using System.Linq;
using RedCoreApi.Models;

namespace RedCoreUnitTest
{
    class TestUserDbSet : TestDbSet<user>
    {
        public override user Find(params object[] keyValues)
        {
            return this.SingleOrDefault(user => user.userid == (int)keyValues.Single());
        }
    }
}
