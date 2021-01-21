namespace FSLib.App.SimpleUpdater.Generator.ZipBuilder
{
	enum ZipTaskState
	{
		Queue = 0,
		FileHashing = 1,
		PackageBuilding = 2,
		PackageHashing = 3,
		Done = 4
	}
}
