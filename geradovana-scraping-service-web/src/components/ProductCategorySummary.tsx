function ProductCategorySummary({
  type,
  amount,
  averagePrice,
}: ProductCategorySummaryProps) {
  return (
    <div className="col">
      <div className="card" style={{ width: "18rem;" }}>
        <div className="card-body">
          <h5 className="card-title">Type: {type}</h5>
          <p className="card-text">Amount: {amount}</p>
          <p className="card-text">Average price: {averagePrice.toFixed(2)}</p>
        </div>
      </div>
    </div>
  );
}

export default ProductCategorySummary;
