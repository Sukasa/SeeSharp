# See Sharp


## A fast cross-platform map renderer

See Sharp is a .NET (C#)-based map rendering program based on Justin Aquadro's excellent Substrate library.  It currently supports command-line automation and GUI easy-configuration.

Current or planned features include:

1. Map rendering
2. World metrics
3. Sign data exporting


### Map Rendering

See Sharp uses a unique plugin-based rendering core that offers a hybrid of maximum speed and customizability.  Both renderers and palette files are external to the program allowing for customization and configuration of all rendering.

Included with See Sharp is a single default renderer and Vanilla palette.


### World Metrics

See Sharp is slated to include a host of world metrics information, including world metrics data.  Currently the program can output (in CLI only) basic data about the world including chunk extents, spawn area, time of day, etc.  Planned additions are block distribution, biome ratios, growth and development charts, mineral wealth distributions, and more.

This feature is incomplete.


### Sign Data Exporting

See Sharp is set up to allow plugin-based sign data to be collated from a world and exported in XML format.  This can be useful for including "Point of Interest" markers, by placing specially-formatted signs in the world.

This feature is incomplete.