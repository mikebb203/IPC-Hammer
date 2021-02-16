<?xml version="1.0"?>
<LogProfile xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ParameterGroup Dpid="0xFE">
    <Parameter Name="Engine Speed" DefineBy="Pid" ByteCount="2" Address="0xC">
      <Conversion Name="RPM" Expression="x*.25" />
    </Parameter>
    <Parameter Name="Mass Air Flow" DefineBy="Pid" ByteCount="2" Address="0x10">
      <Conversion Name="g/s" Expression="x/100" />
    </Parameter>
    <Parameter Name="Manifold Absolute Pressure" DefineBy="Pid" ByteCount="1" Address="0xB">
      <Conversion Name="kpa" Expression="x" />
    </Parameter>
    <Parameter Name="Throttle Position Sensor" DefineBy="Pid" ByteCount="1" Address="0x11">
      <Conversion Name="%" Expression="x/2.56" />
    </Parameter>
  </ParameterGroup>
  <ParameterGroup Dpid="0xFD">
    <Parameter Name="Address FF8800" DefineBy="Address" ByteCount="2" Address="0xFF8800">
      <Conversion Name="Raw" Expression="0x" />
    </Parameter>
    <Parameter Name="Address FF8802" DefineBy="Address" ByteCount="2" Address="0xFF8802">
      <Conversion Name="Raw" Expression="0x" />
    </Parameter>
    <Parameter Name="Ignition Advance Multiplier" DefineBy="Address" ByteCount="2" Address="0xFF8250">
      <Conversion Name="Raw" Expression="0x" />
    </Parameter>
  </ParameterGroup>
</LogProfile>