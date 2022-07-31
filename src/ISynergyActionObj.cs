namespace NP.Synergy
{
    internal interface ISynergyActionObj
    {
        /// connects the property propName of the action object to the synergy assembly cell
        internal void ConnectWithCell(Cell cell, string actionObjDataPointName);
    }
}
