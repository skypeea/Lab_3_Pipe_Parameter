using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3_Pipe_Parameter
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedRefList = uidoc.Selection.PickObjects(ObjectType.Element, "Выберите трубы");

        
            using (Transaction ts = new Transaction(doc, "Set parameters"))
            {
                ts.Start();

                foreach (var selectedElemnt in selectedRefList)
                {
                    Element element = doc.GetElement(selectedElemnt);
                    if (element is Pipe)
                    {
                        Pipe oPipe = (Pipe)element;
                        Parameter length = oPipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                        Parameter stockLength = oPipe.LookupParameter("Длина с запасом");
                        stockLength.Set(length.AsDouble() * 1.1);
                    }
                }

                ts.Commit();
            }

            return Result.Succeeded;
        }
    }
}
