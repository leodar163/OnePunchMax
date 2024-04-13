namespace Inputs
{
    public static class InputsUtility
    {
        private static MainControls _mainControls;

        public static MainControls MainControls
        {
            get
            {
                if (_mainControls == null)
                {
                    _mainControls = new MainControls();
                    _mainControls.Enable();
                }

                return _mainControls;
            }
        }
    }
}