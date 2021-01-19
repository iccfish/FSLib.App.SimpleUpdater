namespace FSLib.App.SimpleUpdater.Generator.ZipBuilder
{
	enum ZipTaskState
	{
		Queue,
		FileHashing,
		PackageBuilding,
		PackageHashing,
		Done
	}
}
