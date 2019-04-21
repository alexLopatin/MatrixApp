using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursachSAS.Controller;
using KursachSAS.View;
using KursachSAS.Model;

namespace KursachSAS.Command
{
    class ChangeOperationTypeCommand<T> : ICommand where T : struct
    {
        private OperationView opView;
        public IOperable<T> newType { get; set; }
        private IOperable<T> oldType;
        public ChangeOperationTypeCommand(OperationView opView, IOperable<T> newType)
        {
            this.opView = opView;
            this.newType = newType;
        }
        public ChangeOperationTypeCommand(OperationView opView)
        {
            this.opView = opView;
        }

        public void Execute()
        {
            oldType = OperationController<T>.GetOperationType(opView);
            OperationController<T>.ChangeOperationType(opView, newType);
        }

        public void Undo()
        {
            OperationController<T>.ChangeOperationType(opView, oldType);
        }
    }
}
