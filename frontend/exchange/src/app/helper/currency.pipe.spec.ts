import { CurrencyPipe } from './currency.pipe';

describe('CurrencyPipeUsPipe', () => {
  it('create an instance', () => {
    const pipe = new CurrencyPipe();
    expect(pipe).toBeTruthy();
  });
});
