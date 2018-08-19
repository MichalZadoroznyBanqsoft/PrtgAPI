﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrtgAPI.Tests.UnitTests.ObjectTests
{
    [TestClass]
    public class TypeTests
    {
        #region Sensor Target

        [TestMethod]
        public void SensorTarget_ReferenceEquals_SensorTarget()
        {
            ExeFileTarget target1 = "test.ps1";
            ExeFileTarget target2 = target1;

            Assert.IsTrue(target1.Equals(target2));
        }

        [TestMethod]
        public void SensorTarget_ValueEquals_SensorTarget()
        {
            ExeFileTarget target1 = "test.ps1";
            ExeFileTarget target2 = "test.ps1";
            ExeFileTarget target3 = "test.ps2";

            Assert.IsTrue(target1.Equals(target2));
            Assert.IsFalse(target1.Equals(target3));
        }

        [TestMethod]
        public void SensorTarget_HashCodeEquals_SensorTarget()
        {
            ExeFileTarget target1 = "test.ps1";
            ExeFileTarget target2 = "test.ps1";

            var target1Hash = target1.GetHashCode();
            var target2Hash = target2.GetHashCode();

            Assert.AreEqual(target1Hash, target2Hash);
        }
        
        [TestMethod]
        public void ScanningInterval_HasCorrectHashCode()
        {
            ScanningInterval interval1 = ScanningInterval.OneHour;
            ScanningInterval interval2 = new TimeSpan(1, 0, 0);

            var code1 = interval1.GetHashCode();
            var code2 = interval2.GetHashCode();

            Assert.AreEqual(code1, code2);

            var timeSpan = new TimeSpan(1, 0, 0);

            Assert.AreNotEqual(code2, timeSpan);
        }

        #endregion
        #region Device Template

        [TestMethod]
        public void DeviceTemplate_ReferenceEquals_DeviceTemplate()
        {
            DeviceTemplate template1 = new DeviceTemplate("file.odt|File||");
            DeviceTemplate template2 = template1;

            Assert.IsTrue(template1.Equals(template2));
        }

        [TestMethod]
        public void DeviceTemplate_ValueEquals_DeviceTemplate()
        {
            DeviceTemplate template1 = new DeviceTemplate("file1.odt|File 1||");
            DeviceTemplate template2 = new DeviceTemplate("file1.odt|File 1||");
            DeviceTemplate template3 = new DeviceTemplate("file2.odt|File 2||");

            Assert.IsTrue(template1.Equals(template2));
            Assert.IsFalse(template1.Equals(template3));
        }

        [TestMethod]
        public void DeviceTemplate_HashCodeEquals_DeviceTemplate()
        {
            DeviceTemplate template1 = new DeviceTemplate("file.odt|File||");
            DeviceTemplate template2 = new DeviceTemplate("file.odt|File||");

            var template1Hash = template1.GetHashCode();
            var template2Hash = template2.GetHashCode();

            Assert.AreEqual(template1Hash, template2Hash);
        }

        [TestMethod]
        public void DeviceTemplate_StringEquals_DeviceTemplate()
        {
            DeviceTemplate template1 = new DeviceTemplate("file.odt|File||");
            DeviceTemplate template2 = new DeviceTemplate("file.odt|File||");

            Assert.AreEqual(template1.ToString(), template2.ToString());
        }

        #endregion
        #region Schedule

        [TestMethod]
        public void Schedule_ReferenceEquals_Schedule()
        {
            Schedule schedule1 = new Schedule("623|Saturdays [GMT+0800]|");
            Schedule schedule2 = schedule1;

            Assert.IsTrue(schedule1.Equals(schedule2));
        }

        [TestMethod]
        public void Schedule_ValueEquals_Schedule()
        {
            Schedule schedule1 = new Schedule("623|Saturdays [GMT+0800]|");
            Schedule schedule2 = new Schedule("623|Saturdays [GMT+0800]|");
            Schedule schedule3 = new Schedule("622|Sundays [GMT+0800]|");

            Assert.IsTrue(schedule1.Equals(schedule2));
            Assert.IsFalse(schedule1.Equals(schedule3));
        }

        [TestMethod]
        public void Schedule_HashCodeEquals_Schedule()
        {
            Schedule schedule1 = new Schedule("623|Saturdays [GMT+0800]|");
            Schedule schedule2 = new Schedule("623|Saturdays [GMT+0800]|");

            var schedule1Hash = schedule1.GetHashCode();
            var schedule2Hash = schedule2.GetHashCode();

            Assert.AreEqual(schedule1Hash, schedule2Hash);
        }

        #endregion
        #region IShallowCloneable

        [TestMethod]
        public void IShallowCloneable_PrtgObjectParameters_CloneFully()
        {
            CloneTable<PrtgObject, PrtgObjectParameters>(new PrtgObjectParameters(), i =>
            {
                if (i.Name == "StartOffset")
                    return true;

                return false;
            });
        }

        [TestMethod]
        public void IShallowCloneable_SensorParameters_CloneFully()
        {
            CloneTable<Sensor, SensorParameters>(new SensorParameters
            {
                Status = new[] {Status.Up}
            }, i =>
            {
                if (i.Name == "StartOffset")
                    return true;

                return false;
            });
        }

        [TestMethod]
        public void IShallowCloneable_DeviceParameters_CloneFully()
        {
            CloneTable<Device, DeviceParameters>(new DeviceParameters(), i =>
            {
                if (i.Name == "StartOffset")
                    return true;

                return false;
            });
        }

        [TestMethod]
        public void IShallowCloneable_GroupParameters_CloneFully()
        {
            CloneTable<Group, GroupParameters>(new GroupParameters(), i =>
            {
                if (i.Name == "StartOffset")
                    return true;

                return false;
            });
        }

        [TestMethod]
        public void IShallowCloneable_ProbeParameters_CloneFully()
        {
            CloneTable<Probe, ProbeParameters>(new ProbeParameters(), i =>
            {
                if (i.Name == "StartOffset")
                    return true;

                if (i.Name == "Content")
                    return true;

                return false;
            });
        }

        [TestMethod]
        public void IShallowCloneable_LogParameters_CloneFully()
        {
            CloneTable<Log, LogParameters>(new LogParameters(3000)
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(-1),
                RecordAge = RecordAge.LastMonth
            });
        }

        [TestMethod]
        public void IShallowCloneable_SensorHistoryParameters_CloneFully()
        {
            Clone(new SensorHistoryParameters(1001, 30, DateTime.Now, DateTime.Now.AddHours(-1), 300)
            {
                Page = 2,
                PageSize = 250,
                Cookie = true
            }, i =>
            {
                if (i.Name == "StartOffset")
                    return true;

                return false;
            });
        }

        private void CloneTable<TObject, TParam>(TParam parameters, Func<MemberInfo, bool> customHandler = null)
            where TObject : ITableObject, IObject
            where TParam : TableParameters<TObject>, IShallowCloneable<TParam>
        {
            if (parameters.SearchFilters != null)
            {
                var old = parameters.SearchFilters.ToList();

                old.Add(new SearchFilter(Property.Id, 3000));

                parameters.SearchFilters = old.ToList();
            }
            else
                parameters.SearchFilters = new List<SearchFilter> {new SearchFilter(Property.Id, 3000)};

            parameters.Count = 15;
            parameters.Page = 2;
            parameters.PageSize = 250;
            parameters.SortBy = Property.Id;
            parameters.SortDirection = SortDirection.Descending;
            parameters.Cookie = true;

            Clone(parameters, customHandler);
        }

        private void Clone<TParam>(TParam parameters, Func<MemberInfo, bool> customHandler = null) where TParam : IShallowCloneable<TParam>
        {
            AssertEx.AllPropertiesAndFieldsAreNotDefault(parameters, i =>
            {
                if (i.Name == "PageSize" && i.GetValue(parameters).Equals(500))
                    Assert.Fail("Property 'PageSize' did not have a value.");

                if (customHandler != null)
                    return customHandler(i);

                return false;
            });

            try
            {
                AssertEx.AllPropertiesAndFieldsAreEqual(parameters, parameters);
            }
            catch (AssertFailedException ex)
            {
                throw new AssertFailedException($"Source object was not self equatable: {ex.Message}", ex);
            }

            var clone = ((IShallowCloneable<TParam>)parameters).ShallowClone();

            AssertEx.AllPropertiesAndFieldsAreEqual(parameters, clone);
        }

        [TestMethod]
        public void All_IShallowCloneable_Types_HaveTests()
        {
            var types = typeof(IShallowCloneable).Assembly.GetTypes().Where(t => typeof(IShallowCloneable).IsAssignableFrom(t) && !t.IsInterface).ToList();

            var expected = types.Select(t => $"IShallowCloneable_{t.Name}_CloneFully").ToList();

            var actual = GetType().GetMethods().Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null).ToList();

            var missing = expected.Where(e => actual.All(m => m.Name != e)).OrderBy(m => m).ToList();

            if (missing.Count > 0)
                Assert.Fail($"{missing.Count} tests are missing: " + string.Join(", ", missing));
        }

        #endregion
        [TestMethod]
        public void AllString_FilterProperties_HaveStringFilterHandler()
        {
            AllPropertiesOfTypeHaveFilterHandler<StringFilterHandler>(
                p => p.PropertyType == typeof(string) || p.PropertyType == typeof(string[]),
                v => v != Property.Url && v != Property.Condition
            );
        }

        [TestMethod]
        public void AllTimeSpan_FilterProperties_HaveTimeSpanValueConverter()
        {
            var exclusions = new[]
            {
                Property.Interval //Only requires padding to 10 spaces. Covered by ZeroPaddingConverter
            };

            AllPropertiesOfTypeHaveValueConverter<TimeSpanConverter>(p => p.PropertyType == typeof(TimeSpan) || p.PropertyType == typeof(TimeSpan?), exclusions);
        }

        [TestMethod]
        public void AllDateTime_FilterProperties_HaveDateTimeValueConverter()
        {
            AllPropertiesOfTypeHaveValueConverter<DateTimeConverter>(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));
        }

        private void AllPropertiesOfTypeHaveFilterHandler<THandler>(Func<PropertyInfo, bool> typeFilter, Func<Property, bool> exclusions)
        {
            var values = GetFilterPropertiesForPrtgObjectProperties(typeFilter).
                Where(exclusions).ToList();

            foreach (var value in values)
            {
                var handler = value.GetEnumAttribute<FilterHandlerAttribute>();

                Assert.IsNotNull(handler, $"Property '{value}' does not have a {typeof(THandler).Name}");
                
                Assert.IsTrue(handler.Handler is THandler, $"Filter handler on property {value} was a {handler.Handler.GetType().Name} instead of a {typeof(THandler).Name}");
            }
        }

        private void AllPropertiesOfTypeHaveValueConverter<TConverter>(Func<PropertyInfo, bool> typeFilter, params Property[] exclusions)
        {
            var values = GetFilterPropertiesForPrtgObjectProperties(typeFilter).
                Where(v => v != Property.Url && !exclusions.Contains(v)).ToList();

            foreach (var value in values)
            {
                var converter = value.GetEnumAttribute<ValueConverterAttribute>();

                Assert.IsNotNull(converter, $"Property '{value}' does not have a {typeof(TConverter).Name}");

                Assert.IsTrue(converter.Converter is TConverter, $"Filter handler on property {value} was a {converter.Converter.GetType().Name} instead of a {typeof(TConverter).Name}");
            }
        }

        private List<Property> GetFilterPropertiesForPrtgObjectProperties(Func<PropertyInfo, bool> typeFilter)
        {
            var types = typeof(PrtgObject).Assembly.GetTypes().Where(t => typeof(PrtgObject).IsAssignableFrom(t)).ToList();

            var properties = types.SelectMany(t => t.GetProperties().Where(typeFilter)).ToList();

            var validProperties = properties.Where(p => p.GetCustomAttributes(typeof(PropertyParameterAttribute), false).Length > 0).ToList();

            var values = validProperties
                .Select(p => p.GetCustomAttributes(typeof(PropertyParameterAttribute), false).First())
                .Cast<PropertyParameterAttribute>()
                .Select(a =>
                {
                    Property prop;

                    if (Enum.TryParse(a.Name, true, out prop))
                        return (Property?)prop;

                    return null;
                })
                .Where(e => e != null)
                .Cast<Property>()
                .Distinct()
                .ToList();

            return values;
        }
    }
}
