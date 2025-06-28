using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using StockPredictionAPI.Models;

namespace StockPredictionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PredictionController : ControllerBase, IDisposable
    {
        private readonly InferenceSession _modelSession;
        private readonly InferenceSession _scalerSession;
        private bool _disposed = false;

        public PredictionController()
        {
            // Loading ONNX models
            _scalerSession = new InferenceSession("MlModels/scaler.onnx");
            _modelSession = new InferenceSession("MlModels/linear_regression_model.onnx");
        }
        
        [HttpPost("predict")]
        public ActionResult<object> Predict([FromBody] StockInput input)
        {
            try
            {
                // Preparing input data ( Open, High, Year, Low)
                var inputData = new float[] { input.Open, input.High, input.Year, input.Low };
                
                // Creating tensor for scaling
                var inputTensor = new DenseTensor<float>(inputData, new[] { 1, 4 });
                var scalerInputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };

                // Applying scaling
                using var scalerResults = _scalerSession.Run(scalerInputs);
                var scaledData = scalerResults.First().AsTensor<float>();

                // Creating tensor for prediction
                var predictionInputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", scaledData)
                };

                // Making prediction
                using var modelResults = _modelSession.Run(predictionInputs);
                var prediction = modelResults.First().AsTensor<float>().First();

                return Ok(new { Close = prediction });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}