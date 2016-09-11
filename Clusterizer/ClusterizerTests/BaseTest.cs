using Clusterizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterizerTests
{
    public class BaseTest
    {
        protected IKernel kernel;

        [TestInitialize]
        public void Initialize()
        {
            kernel = new StandardKernel(new DIModule());
        }
    }
}
