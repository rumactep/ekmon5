using System.Collections.Generic;

namespace smartlink.JsonData {

    //public class ESs : List<ES> { public void Visit(IVisitor v) { v.VisitES(this); } }

    public class JSONS {
        public AnalogInputs ANALOGINPUTS { get; set; } = new();
        public DigitalInputs DIGITALINPUTS { get; set; } = new();
        public Counters COUNTERS { get; set; } = new();
        public Converters CONVERTERS { get; set; } = new();
        public DigitalOutputs DIGITALOUTPUTS { get; set; } = new();

        public CalculatedAnalogInputs CALCULATEDANALOGINPUTS { get; set; } = new();
        public SpecialProtections SPECIALPROTECTIONS { get; set; } = new();
        public AnalogOutputs ANALOGOUTPUTS { get; set; } = new();

        public SPMs SPM2 { get; set; } = new();

        // ES is not implemented yet
        public List<object> ES { get; set; } = new();
        public ServicePlans SERVICEPLAN { get; set; } = new();
        public Devices DEVICE { get; set; } = new();

        public void Accept(IVisitor visitor) {
            ANALOGINPUTS.Visit(visitor);
            DIGITALINPUTS.Visit(visitor);
            COUNTERS.Visit(visitor);
            CONVERTERS.Visit(visitor);
            DIGITALOUTPUTS.Visit(visitor);

            CALCULATEDANALOGINPUTS.Visit(visitor);
            SPECIALPROTECTIONS.Visit(visitor);
            ANALOGOUTPUTS.Visit(visitor);
            SPM2.Visit(visitor);
            // ES not implemented
            // ES.Visit(visitor);
            SERVICEPLAN.Visit(visitor);
            DEVICE.Visit(visitor);
        }
    }
}