using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;

namespace NavmeshCollector
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "NavmeshCollector.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            int count = 0;

            foreach (var Navmesh in state.LoadOrder.PriorityOrder.NavigationMesh().WinningContextOverrides(state.LinkCache))
            {
                if (Navmesh.Record.Data == null)
                {
                    continue;
                }

                System.Console.WriteLine("Collecting Navmesh " + Navmesh.Record.FormKey.ToString());

                Navmesh.GetOrAddAsOverride(state.PatchMod);
                count++;
            }

            System.Console.WriteLine("Done! Collected " + count.ToString() + " navmeshes.");
        }
    }
}
